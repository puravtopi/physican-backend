using Dapper;
using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;
using System.Data;
using System.Reflection.Emit;


namespace PhysicianBooking.Services.Implementation
{
    public class LeadsRepository : ILeadsRepository
    {
        private readonly IDbConnection _db;

        public LeadsRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task SaveLeads(LeadsModal modal)
        {
            string sql = @"
    INSERT INTO tbl_leads (place_id, ip_address, created_date)
    SELECT @place_id, @ip_address, @created_date
    WHERE NOT EXISTS (
        SELECT 1 FROM tbl_leads 
        WHERE place_id = @place_id 
          AND ip_address = @ip_address 
          AND created_date::date = @created_date::date
    );";
            modal.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            await _db.ExecuteAsync(sql, new
            {
                place_id = (Int64)modal.PlaceId,
                ip_address = (string)modal.IP_Address,
                created_date = modal.CreatedDate == default ? DateTime.UtcNow.Date : modal.CreatedDate.ToDateTime(TimeOnly.MinValue)
            });

        }


        public async Task<PagedResult<LeadsModal>> GetAllLeadsAsync(
     string search = "",
     int pageNumber = 1,
     string fromDate = "",
     string toDate = "",
     int pageSize = 10)
        {
            int offset = (pageNumber - 1) * pageSize;
            List<string> filters = new List<string>();

            // Use DynamicParameters to avoid sending unused/invalid parameters
            var parameters = new DynamicParameters();
            parameters.Add("pageSize", pageSize);
            parameters.Add("offset", offset);

            // 1. Handle Text Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                filters.Add("(P.name::text ILIKE @search or P.specialty::text ILIKE @search)");
                // Wildcards are ONLY for text search
                parameters.Add("search", $"%{search}%");
            }

            // 2. Handle Date Range (FIXED: Removed wildcards)
            if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(toDate))
            {
                // Use standard casting for better performance/readability
                filters.Add("L.created_date >= @fromDate::date AND L.created_date < (@toDate::date + interval '1 day')");

                // Send raw strings "YYYY-MM-DD"
                parameters.Add("fromDate", fromDate);
                parameters.Add("toDate", toDate);
            }

            string whereClause = filters.Count > 0
                ? "WHERE " + string.Join(" AND ", filters)
                : string.Empty;

            string paginationClause = pageSize > 0
                ? "LIMIT @pageSize OFFSET @offset"
                : string.Empty;

            // 3. Count Query
            string countSql = $@"
        SELECT COUNT(*)
        FROM tbl_leads L
        INNER JOIN tbl_places P ON L.place_id = P.""id"" 
        {whereClause};";

            // 4. Data Query
            string dataSql = $@"
        SELECT
            L.""Id"",
            L.place_id as PlaceId, 
            L.ip_address as IP_Address,
            L.created_date::date as CreatedDate,
            P.name as Place,
            P.specialty as Specialty
        FROM tbl_leads L
        INNER JOIN tbl_places P ON L.place_id = P.""id"" 
        {whereClause}
        ORDER BY L.created_date DESC
        {paginationClause};";

            var totalRecords = await _db.ExecuteScalarAsync<int>(countSql, parameters);
             var data = await _db.QueryAsync<LeadsModal>(dataSql, parameters);

            return new PagedResult<LeadsModal>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize == 0 ? totalRecords : pageSize
            };
        }
    }
}