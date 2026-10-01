using Dapper;
using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;
using System.Data;
using System.Net.Http;


namespace PhysicianBooking.Services.Implementation
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly IDbConnection _db;

        public PlaceRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task SavePlace(dynamic result)
        {
            string sql = @"
        INSERT INTO places
        (
            place_id,
            name,
            address,
            latitude,
            longitude,
            rating,
            user_ratings_total
        )
        VALUES
        (
            @place_id,
            @name,
            @address,
            @lat,
            @lng,
            @rating,
            @total
        )
        ON CONFLICT (place_id)
        DO NOTHING";

            await _db.ExecuteAsync(sql, new
            {
                place_id = (string)result.place_id,
                name = (string)result.name,
                address = (string)result.formatted_address,
                lat = (decimal)result.geometry.location.lat,
                lng = (decimal)result.geometry.location.lng,
                rating = (decimal?)result.rating,
                total = (int?)result.user_ratings_total
            });
        }

        public async Task<PagedResult<PhysicianModal>> GetAllPhysicianAsync(
            string search = "",
       string category = "",
       string zipcode = "",
       string city = "",
       string state = "",
       decimal? minRating = null, // Added rating parameter
       int pageNumber = 1,
       int pageSize = 10)
        {
            int offset = (pageNumber - 1) * pageSize;

            // 1. Build dynamic WHERE clause
            List<string> filters = new List<string>();

            // Global Search Filter (applied across multiple columns)
            if (!string.IsNullOrWhiteSpace(search))
            {
                // We group these with OR and wrap in parentheses
                filters.Add("(name ILIKE @search OR specialty::text ILIKE @search OR location::text ILIKE @search)");
            }

            if (!string.IsNullOrWhiteSpace(category))
                filters.Add("specialty::text ILIKE @category");

            if (!string.IsNullOrWhiteSpace(zipcode))
                filters.Add("zip_code::text ILIKE @zipcode or city::text ILIKE @zipcode or state::text ILIKE @zipcode");

            if (!string.IsNullOrWhiteSpace(city))
                filters.Add("city::text ILIKE @city");

            if (!string.IsNullOrWhiteSpace(state))
                filters.Add("state::text ILIKE @state");

            if (minRating.HasValue && minRating > 0)
                filters.Add("rating >= @minRating"); // Filter for minimum rating

            string whereClause = filters.Count > 0
                ? "WHERE " + string.Join(" AND ", filters)
                : string.Empty;

            // 2. Count Query
            string countSql = $@"
        SELECT COUNT(*)
        FROM tbl_places
        {whereClause};";

            // 3. Data Query
            string dataSql = $@"
        SELECT
            Id,
            place_id as PlaceId,
            name,
            location,
            latitude,
            longitude,
            rating,
            reviews as reviewCount,
            zip_code as zipCode,
            specialty,
            hospital,
            experience,
            bio 
        FROM tbl_places
        {whereClause}
        ORDER BY name ASC,rating DESC
        LIMIT @pageSize
        OFFSET @offset;";

            var parameters = new
            {
                category = $"%{category}%",
                search = $"%{search}%",
                zipcode = $"%{zipcode}%",
                city = $"%{city}%",
                state = $"%{state}%",
                minRating = minRating ?? 0, // Pass 0 if null
                pageSize,
                offset
            };

            var totalRecords = await _db.ExecuteScalarAsync<int>(countSql, parameters);
            var data = await _db.QueryAsync<PhysicianModal>(dataSql, parameters);

            return new PagedResult<PhysicianModal>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PhysicianModal> GetPhysicianByIdAsync(int Id)
        {


            string dataSql = $@"
        SELECT
            Id,
            place_id as PlaceId,
            name,
            location,
            latitude,
            longitude,
            rating,
            reviews as reviewCount,
            zip_code as zipCode,
            specialty,
            hospital,
            experience,
            phone,
            bio,
            website
        FROM tbl_places
        where id={Id}
        ;";



            var data = await _db.QueryAsync<PhysicianModal>(
                dataSql);

            return data?.FirstOrDefault();
        }

        public async Task<PagedResult<ReviewModal>> GetPhysicianReviewsAsync(int Id, int pageNumber = 1, int pageSize = 5)
        {
            int offset = (pageNumber - 1) * pageSize;

            string whereClause = string.Empty;


            whereClause = @"
            WHERE place_id=@Id
                ";


            string countSql = $@"
        SELECT COUNT(*)
        FROM tbl_reviews
        {whereClause};";

            string dataSql = $@"
        SELECT
            'Id',
           place_id as PlaceId, author, rating, date::date, text
        FROM tbl_reviews
        {whereClause}
        ORDER BY rating DESC
        LIMIT @pageSize
        OFFSET @offset;";

            var parameters = new
            {
                Id = Id,
                pageSize,
                offset

            };

            var totalRecords = await _db.ExecuteScalarAsync<int>(
                countSql,
                parameters);

            var data = await _db.QueryAsync<ReviewModal>(
                dataSql,
                parameters);

            return new PagedResult<ReviewModal>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<List<string>> GetPhysicianWorkingHours(Int64 Id)
        {

            string dataSql = @"
        SELECT details 
        FROM tbl_working_hours 
        WHERE place_id = @Id;";

            var data = await _db.QueryAsync<string>(dataSql, new { Id = Id });

            return data.ToList(); // Convert IEnumerable to List
        }
    }
}
