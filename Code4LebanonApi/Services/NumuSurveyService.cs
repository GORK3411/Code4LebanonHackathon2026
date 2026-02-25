using Code4LebanonApi.Models;
using System.Net.Http;
using System.Runtime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Code4LebanonApi.Services
{


    public class NumuSurveyService
    {
        private readonly HttpClient _httpClient;

        public NumuSurveyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://numu-survey.codeforlebanon.com/");
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", "cfl_7f3a9b2c8d1e4f6a0b5c3d9e2f4a1b3c");
        }

        // =========================
        // SURVEYS
        // =========================

        // GET /api/surveys
        // Update your GetAllSurveysAsync
        public async Task<List<Survey>> GetAllSurveysAsync(int page = 1, int limit = 30)
        {
            var response = await _httpClient.GetAsync($"/api/surveys?page={page}&limit={limit}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            // Deserialize to ApiResponse<SurveyListData>
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<SurveyListData>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Return the list of surveys inside "data.surveys"
            return apiResponse?.Data?.Surveys ?? new List<Survey>();
        }

        // GET /api/surveys/{id}
        public async Task<Survey> GetSurveyByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"api/surveys/{id}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Survey>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return apiResponse?.Data; // this is your Survey object
        }

        // GET /api/surveys/slug/{slug}
        public async Task<Survey> GetSurveyBySlugAsync(string slug)
        {
            var response = await _httpClient.GetAsync($"api/surveys/slug/{slug}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Survey>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return apiResponse?.Data;
        }

        // =========================
        // RESPONSES
        // =========================

        // GET /api/responses
        public async Task<List<SurveyResponse>> GetResponsesAsync(
            DateTime startDate,
            DateTime endDate,
            int page = 1,
            int limit = 30)
        {
            string start = startDate.ToString("yyyy-MM-dd");
            string end = endDate.ToString("yyyy-MM-dd");

            string url = $"api/responses?start_date={start}&end_date={end}&page={page}&limit={limit}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<SurveyResponseListData>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return apiResponse?.Data?.Responses ?? new List<SurveyResponse>();
        }


        // GET /api/responses/{id}
        public async Task<SurveyResponse> GetResponseByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"api/responses/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<SurveyResponse>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return apiResponse?.Data;
        }


        // GET /api/responses/last-response-date
        public async Task<LastestResponseClass?> GetLastResponseDateAsync()
        {
            var response = await _httpClient.GetAsync("api/responses/last-response-date");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<LastestResponseClass>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return apiResponse?.Data;
        }
        private class ApiResponse<T>
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }

            [JsonPropertyName("data")]
            public T Data { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("timestamp")]
            public DateTime Timestamp { get; set; }
        }

        // Wrapper for the "data" object in the list response
        public class SurveyListData
        {
            [JsonPropertyName("surveys")]
            public List<Survey> Surveys { get; set; }

            [JsonPropertyName("pagination")]
            public Pagination Pagination { get; set; }
        }

        public class SurveyResponseListData
        {
            [JsonPropertyName("responses")]
            public List<SurveyResponse> Responses { get; set; }

            [JsonPropertyName("pagination")]
            public Pagination Pagination { get; set; }
        }

        public class Pagination
        {
            [JsonPropertyName("page")]
            public int Page { get; set; }

            [JsonPropertyName("limit")]
            public int Limit { get; set; }

            [JsonPropertyName("total")]
            public int Total { get; set; }

            [JsonPropertyName("totalPages")]
            public int TotalPages { get; set; }

            [JsonPropertyName("hasNextPage")]
            public bool HasNextPage { get; set; }

            [JsonPropertyName("hasPrevPage")]
            public bool HasPrevPage { get; set; }
        }

        // Backwards-compatible helper used by existing controllers
        public async Task<string> GetDataAsync()
        {
            // Default to fetching first page of surveys
            return await GetAllSurveysAsync();
        }
    }
}
