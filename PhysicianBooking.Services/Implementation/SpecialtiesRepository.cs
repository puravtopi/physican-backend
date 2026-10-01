using Dapper;
using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Implementation
{
    public class SpecialtiesRepository : ISpecialtiesRepository
    {
        private readonly IDbConnection _db;

        public SpecialtiesRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<PagedResult<SpecialtiesModal>> GetSpecialtiesAsync(string searchText, int pageNumber, int pageSize)
        {
            // Define if we should skip pagination
            bool skipPagination = pageSize <= 0;

            int offset = skipPagination ? 0 : (pageNumber - 1) * pageSize;

            string whereClause = string.Empty;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                whereClause = "WHERE name ILIKE @search";
            }

            string countSql = $@"
        SELECT COUNT(*) 
        FROM tbl_specialties 
        {whereClause};";

            // Conditionally add the LIMIT and OFFSET clauses
            string paginationClause = skipPagination ? "" : "LIMIT @pageSize OFFSET @offset";

            string dataSql = $@"
        SELECT *
        FROM tbl_specialties
        {whereClause}
        ORDER BY name
        {paginationClause};";

            var parameters = new
            {
                search = $"%{searchText}%",
                pageSize,
                offset
            };

            var totalRecords = await _db.ExecuteScalarAsync<int>(countSql, parameters);
            var data = await _db.QueryAsync<SpecialtiesModal>(dataSql, parameters);

            return new PagedResult<SpecialtiesModal>
            {
                Data = data,
                TotalRecords = totalRecords,
                // If skipped pagination, PageNumber is 1 and PageSize is the total count
                PageNumber = skipPagination ? 1 : pageNumber,
                PageSize = skipPagination ? totalRecords : pageSize
            };
        }

        public async Task<PagedResult<ZipModal>> GetZips(string searchText, int pageNumber, int pageSize)
        {
            // Define if we should skip pagination
            bool skipPagination = pageSize <= 0;

            int offset = skipPagination ? 0 : (pageNumber - 1) * pageSize;

            string whereClause = string.Empty;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                whereClause = "WHERE name ILIKE @search";
            }

            string countSql = $@"
        SELECT COUNT(*) 
        FROM tbl_zipcodes 
        {whereClause};";

            // Conditionally add the LIMIT and OFFSET clauses
            string paginationClause = skipPagination ? "" : "LIMIT @pageSize OFFSET @offset";

            string dataSql = $@"
        SELECT *
        FROM tbl_zipcodes
        {whereClause}
        ORDER BY code
        {paginationClause};";

            var parameters = new
            {
                search = $"%{searchText}%",
                pageSize,
                offset
            };

            var totalRecords = await _db.ExecuteScalarAsync<int>(countSql, parameters);
            var data = await _db.QueryAsync<ZipModal>(dataSql, parameters);

            return new PagedResult<ZipModal>
            {
                Data = data,
                TotalRecords = totalRecords,
                // If skipped pagination, PageNumber is 1 and PageSize is the total count
                PageNumber = skipPagination ? 1 : pageNumber,
                PageSize = skipPagination ? totalRecords : pageSize
            };
        }

        public async Task<PagedResult<string>> GetCities(string searchText, int pageNumber, int pageSize)
        {
            // Define if we should skip pagination
            bool skipPagination = pageSize <= 0;
            int offset = skipPagination ? 0 : (pageNumber - 1) * pageSize;

            // Filter out null/blank values and optionally filter by search text
            string whereClause = "WHERE city IS NOT NULL AND TRIM(city) <> ''";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                whereClause += " AND city ILIKE @search";
            }

            // Count distinct cities
            string countSql = $@"
        SELECT COUNT(DISTINCT city) 
        FROM tbl_places 
        {whereClause};";

            // Conditionally add the LIMIT and OFFSET clauses
            string paginationClause = skipPagination ? "" : "LIMIT @pageSize OFFSET @offset";

            // Select distinct cities ordered alphabetically
            string dataSql = $@"
        SELECT DISTINCT city
        FROM tbl_places
        {whereClause}
        ORDER BY city ASC
        {paginationClause};";

            var parameters = new
            {
                search = $"%{searchText?.Trim()}%",
                pageSize,
                offset
            };

            var totalRecords = await _db.ExecuteScalarAsync<int>(countSql, parameters);
            var data = await _db.QueryAsync<string>(dataSql, parameters);

            return new PagedResult<string>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = skipPagination ? 1 : pageNumber,
                PageSize = skipPagination ? totalRecords : pageSize
            };
        }

        public async Task<PagedResult<string>> GetStates(string searchText, int pageNumber, int pageSize)
        {
            // Define if we should skip pagination
            bool skipPagination = pageSize <= 0;
            int offset = skipPagination ? 0 : (pageNumber - 1) * pageSize;

            // Filter out null/blank values and optionally filter by search text
            string whereClause = "WHERE state IS NOT NULL AND TRIM(state) <> ''";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                whereClause += " AND state ILIKE @search";
            }

            // Count distinct states
            string countSql = $@"
        SELECT COUNT(DISTINCT state) 
        FROM tbl_places 
        {whereClause};";

            // Conditionally add the LIMIT and OFFSET clauses
            string paginationClause = skipPagination ? "" : "LIMIT @pageSize OFFSET @offset";

            // Select distinct states ordered alphabetically
            string dataSql = $@"
        SELECT DISTINCT state
        FROM tbl_places
        {whereClause}
        ORDER BY state ASC
        {paginationClause};";

            var parameters = new
            {
                search = $"%{searchText?.Trim()}%",
                pageSize,
                offset
            };

            var totalRecords = await _db.ExecuteScalarAsync<int>(countSql, parameters);
            var data = await _db.QueryAsync<string>(dataSql, parameters);

            return new PagedResult<string>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = skipPagination ? 1 : pageNumber,
                PageSize = skipPagination ? totalRecords : pageSize
            };
        }
    }
}
