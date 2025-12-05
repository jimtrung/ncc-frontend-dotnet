using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Services
{
    public class UserService
    {
        private readonly HttpClient _http;
        private readonly AuthTokenUtil _tokenUtil;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            Converters = { new JsonStringEnumConverter() }
        };

        public UserService(HttpClient httpClient, AuthTokenUtil authTokenUtil)
        {
            _http = httpClient;
            _http.BaseAddress = new Uri("http://localhost:8080/");
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _tokenUtil = authTokenUtil;
        }


        public int GetAllUsers()
        {
            var token = _tokenUtil.LoadAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "user/all");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _http.Send(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching users: {response.StatusCode}");
                return 0;
            }

            var body = response.Content.ReadAsStringAsync().Result;

            // Nếu body rỗng => trả về 0
            if (string.IsNullOrWhiteSpace(body))
                return 0;

            try
            {
                // Deserialize trực tiếp số nguyên
                return JsonSerializer.Deserialize<int>(body);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to parse user count: {ex.Message}");
                return 0;
            }
        }

    }
}
