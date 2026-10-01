using Dapper;
using Microsoft.Extensions.Configuration;
using PhysicianBooking.Domain;
using PhysicianBooking.Domain.Modal;
using PhysicianBooking.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianBooking.Services.Implementation
{
    public class GooglePlacesService : IGooglePlacesService
    {
        #region Variables
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IDbConnection _db;
        #endregion

        #region Ctor
        public GooglePlacesService(HttpClient httpClient,
            IDbConnection db,
                                   IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _db = db;
        }
        #endregion

        #region Methods
        public async Task<string> SearchPlaces(string query)
        {
            var apiKey = _config["GoogleApi:ApiKey"];

            var url =
                $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={query}&key={apiKey}";

            return await _httpClient.GetStringAsync(url);
        }

        //public async Task<GooglePlacesResponse> SearchAndSaveBusinesses(string category, string zipCode)
        //{
        //    var _apiKey = _config["GoogleApi:ApiKey"];
        //    var url = "https://places.googleapis.com/v1/places:searchText";

        //    var allBusinessResults = new List<Domain.Place>(); // Use your actual Place DTO class
        //    string currentPageToken = null;

        //    // 1. Set Required Headers (Done once outside the loop)
        //    _httpClient.DefaultRequestHeaders.Clear();
        //    _httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", _apiKey);

        //    // CRITICAL: Include "nextPageToken" in the FieldMask to receive it in the response
        //    _httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", "places.id,places.displayName,places.formattedAddress," +
        //        "places.reviews,places.nationalPhoneNumber,places.websiteUri,places.rating,places.userRatingCount,nextPageToken");

        //    do
        //    {
        //        // 2. Prepare Request Body
        //        var requestBody = new
        //        {
        //            textQuery = $"{category} in {zipCode}",
        //            includedType = "doctor",
        //            pageSize = 20, // Use pageSize (max 20) instead of maxResultCount
        //            pageToken = currentPageToken // This will be null on the first call
        //        };

        //        // 3. Make API Call
        //        var response = await _httpClient.PostAsJsonAsync(url, requestBody);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            // Handle error (logging, etc.)
        //            break;
        //        }

        //        // 4. Parse Response
        //        var data = await response.Content.ReadFromJsonAsync<GooglePlacesResponse>();

        //        if (data?.Places != null)
        //        {
        //            allBusinessResults.AddRange(data.Places);
        //        }

        //        // 5. Update the token for the next iteration
        //        currentPageToken = data?.NextPageToken;

        //        // Google enforces a small delay before the nextPageToken becomes active.
        //        // If you get errors, uncomment the line below:
        //        // if (!string.IsNullOrEmpty(currentPageToken)) await Task.Delay(1000);

        //    } while (!string.IsNullOrEmpty(currentPageToken));

        //    // 'allBusinessResults' now contains all records found (up to 60)

        //    var reuslt = new GooglePlacesResponse
        //    {
        //        Places = allBusinessResults,
        //        NextPageToken = currentPageToken
        //    };
        //    Int64 pincode = Convert.ToInt64(zipCode);
        //    this.SavePlaces(reuslt, category, pincode);
        //    return reuslt;
        //}

        public async Task<GooglePlacesResponse> SearchAndSaveBusinesses(string category, string zipCode)
        {
            var apiKey = _config["GoogleApi:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException("Google API Key is not configured.");

            var url = "https://places.googleapis.com/v1/places:searchText";
            var allBusinessResults = new List<Domain.Place>();
            string? currentPageToken = null;

            // Field mask explicitly scoped
            //var fieldMask = "places.id,places.displayName,places.formattedAddress," +
            //                "places.reviews,places.nationalPhoneNumber,places.websiteUri," +
            //                "places.rating,places.userRatingCount,nextPageToken";
            var fieldMask = "places.id,places.displayName,places.formattedAddress,places.addressComponents," +
                         "places.reviews,places.nationalPhoneNumber,places.websiteUri," +
                         "places.rating,places.userRatingCount,nextPageToken";

            do
            {
                var requestBody = new
                {
                    textQuery = $"{category} near {zipCode}, USA", // "near" often performs better than "in"
                    pageSize = 20,
                    pageToken = currentPageToken
                    // REMOVED includedType unless your method ONLY handles doctors
                };

                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("X-Goog-Api-Key", apiKey);
                request.Headers.Add("X-Goog-FieldMask", fieldMask);
                request.Content = JsonContent.Create(requestBody);

                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    //_logger.LogError("Google Places API error: {StatusCode} - {Body}", response.StatusCode, errorBody);
                    break;
                }

                var data = await response.Content.ReadFromJsonAsync<GooglePlacesResponse>();

                if (data?.Places != null)
                {
                    // Filter results to only include the exact zip code
                    var filteredPlaces = data.Places.Where(p =>
                        p.AddressComponents != null &&
                        p.AddressComponents.Any(c =>
                             c.LongText == zipCode // or use .Contains if you want to match 48187-1234
                        )
                    ).ToList();
                    allBusinessResults.AddRange(filteredPlaces);
                }

                currentPageToken = data?.NextPageToken;

                // CRITICAL: Keep this uncommented. Google needs ~1.5–2s to index the next page token.
                if (!string.IsNullOrEmpty(currentPageToken))
                {
                    await Task.Delay(1500);
                }

            } while (!string.IsNullOrEmpty(currentPageToken));

            var finalResult = new GooglePlacesResponse
            {
                Places = allBusinessResults,
                NextPageToken = null // fully exhausted
            };

            // Safe parse for non-numeric postal codes (UK, CA, etc.)
            if (long.TryParse(zipCode, out long numericPincode))
            {
                this.SavePlaces(finalResult, category, numericPincode);
            }
            else
            {
                //_logger.LogWarning("Postal code {ZipCode} is non-numeric; saving with 0 or handling separately.", zipCode);
                this.SavePlaces(finalResult, category, 0);
            }

            return finalResult;
        }


        public async Task<bool> GetWeekdayDescriptionsAsync()
        {

            string placeId="";
            Int64 Id=0;


            string dataSql = $@"
        SELECT
            Id,
            place_id as PlaceId
            FROM tbl_places
       
        ;";



            var data = await _db.QueryAsync<PhysicianModal>(
                dataSql);

            if (data != null)
            {

                var _apiKey = _config["GoogleApi:ApiKey"];

                //if (string.IsNullOrWhiteSpace(placeId))
                //    throw new ArgumentException("Place ID cannot be empty.", nameof(placeId));

                foreach (var item in data)
                {

                    placeId = item.PlaceId;
                    Id = item.Id;

                    // Places API (New) Place Details URL
                    var url = $"https://places.googleapis.com/v1/places/{placeId}";

                    // Field mask specifically requests opening hours and place name (optional)
                    var fieldMask = "id,displayName,regularOpeningHours.weekdayDescriptions";

                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("X-Goog-Api-Key", _apiKey);
                    request.Headers.Add("X-Goog-FieldMask", fieldMask);

                    // Timeout safety (20 seconds)
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

                    using var response = await _httpClient.SendAsync(
                        request,
                        HttpCompletionOption.ResponseHeadersRead,
                        cts.Token
                    );

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorBody = await response.Content.ReadAsStringAsync(cts.Token);
                        throw new HttpRequestException(
                            $"Google Places API error ({response.StatusCode}): {errorBody}"
                        );
                    }

                    var place = await response.Content.ReadFromJsonAsync<PlaceDetailsResponse>(cancellationToken: cts.Token);

                    if (place?.RegularOpeningHours != null)
                    {
                        // Save to database
                        this.SaveWorkingHours(place.RegularOpeningHours, Id);
                       
                    }
                }
                return true;
            }
            
            return false;
        }

        #endregion

        #region Private Methods
        private void SavePlaces(GooglePlacesResponse modal, string category, Int64 zipCode)
        {
            try
            {
                string sql = @"
    WITH inserted AS (
    INSERT INTO public.tbl_places (
        place_id, name, location, latitude, longitude, rating, reviews, phone, 
        website, google_maps_url, business_status, created_at, specialty, 
        zip_code, bio, city, state, experience
    )
    SELECT 
        @place_id, @name, @location, @latitude, @longitude, @rating, @reviews, @phone, 
        @website, @google_maps_url, @business_status, @created_at, @specialty, 
        @zip_code, @bio, @city, @state, @experience
    WHERE NOT EXISTS (
        SELECT 1 FROM tbl_places WHERE place_id = @place_id
    )
    RETURNING id
)
SELECT id FROM inserted
UNION ALL
SELECT id FROM tbl_places WHERE place_id = @place_id
LIMIT 1;";
                foreach (var place in modal.Places)
                {
                    string city = "", state = "";
                    if (!string.IsNullOrEmpty(place.FormattedAddress))
                    {
                        // 1. Split by comma
                        var parts = place.FormattedAddress.Split(',');

                        if (parts.Length >= 3)
                        {
                            // The second part is the City
                            city = parts[1].Trim();

                            // The third part is "MI 48150"
                            string statePart = parts[2].Trim();

                            // Split the state part by space to get just "MI"
                            state = statePart.Split(' ')[0];

                        }
                    }

                    var result = _db.ExecuteScalar<Int64>(sql, new
                    {
                        place_id = (string)place.Id,
                        name = (string)place.DisplayName?.Text,
                        created_at = System.DateTime.Now,
                        location = (string)place.FormattedAddress,
                        rating = (double?)place.Rating ?? 0,
                        latitude = (double?)place.Location?.Latitude ?? 0,
                        longitude = (double?)place.Location?.Longitude ?? 0,
                        reviews = (int?)place.UserRatingCount ?? 0,
                        phone = (string?)place.NationalPhoneNumber ?? "",
                        website = (string?)place.WebsiteUri ?? "",
                        google_maps_url = (string?)place.GoogleMapsUri ?? "",
                        business_status = "",
                        specialty = (string)category,
                        zip_code = (Int64)zipCode,
                        bio = (string?)place.EditorialSummary ?? "No description provided",
                        city = city,
                        state = state,
                        experience = (int?)null

                    });

                    this.SaveReviews(place.Reviews, result);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveReviews(List<PhysicianBooking.Domain.Review> modal, Int64 place_id)
        {
            string sql = @"

    INSERT INTO public.tbl_reviews(
	place_id, author, rating, date, text)
    SELECT @place_id, @author, @rating, @date, @text
    ";
            if (modal != null)
            {
                foreach (var review in modal)
                {
                    _db.Execute(sql, new
                    {
                        place_id = (Int64)place_id,
                        author = (string)review?.AuthorAttribution?.DisplayName,
                        rating = (int)review?.Rating,
                        date = (DateTime)review.PublishTime,
                        text = (string?)review?.Text?.Text,

                    });
                }
            }
        }

        private void SaveWorkingHours(RegularOpeningHours modal, Int64 place_id)
        {
            try
            {
                string sql = @"
    INSERT INTO public.tbl_working_hours (
        place_id, details
    )
    VALUES (
        @place_id, @details
    )
   RETURNING ""Id"";";
                foreach (var place in modal.WeekdayDescriptions)
                {

                    var result = _db.ExecuteScalar<Int64>(sql, new
                    {
                        place_id = (Int64)place_id,
                        details = (string)place,

                    });

                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
    }
}
