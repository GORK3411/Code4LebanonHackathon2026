 using System.Net.Http;
 using System.Text.Json;

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
        public async Task<string> GetAllSurveysAsync(int page = 1,int limit = 30)
        {
            var response = await _httpClient.GetAsync($"/api/surveys?page={page}&limit={limit}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // GET /api/surveys/{id}
        public async Task<string> GetSurveyByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/surveys/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // GET /api/surveys/slug/{slug}
        public async Task<string> GetSurveyBySlugAsync(string slug)
        {
            var response = await _httpClient.GetAsync($"api/surveys/slug/{slug}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // =========================
        // RESPONSES
        // =========================

        // GET /api/responses
        public async Task<string> GetResponsesAsync(
    DateTime startDate,
    DateTime endDate,
    int page = 1,
    int limit = 30)
        {
            // Format dates as YYYY-MM-DD
            string start = startDate.ToString("yyyy-MM-dd");
            string end = endDate.ToString("yyyy-MM-dd");

            // Build query string
            string url = $"api/responses?start_date={start}&end_date={end}&page={page}&limit={limit}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        // GET /api/responses/last-response-date
        public async Task<string> GetLastResponseDateAsync()
        {
            var response = await _httpClient.GetAsync("api/responses/last-response-date");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // GET /api/responses/{id}
        public async Task<string> GetResponseByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/responses/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
