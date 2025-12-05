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
    public class ShowtimeService
    {
        private readonly HttpClient _http;
        private readonly AuthTokenUtil _tokenUtil;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public ShowtimeService(HttpClient http, AuthTokenUtil tokenUtil)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:8080/");
            _tokenUtil = tokenUtil;
        }

        // Thêm mới showtime
        public async Task InsertShowtime(Showtime showtime)
        {
            var content = JsonContent.Create(showtime, options: JsonOptions);
            var request = new HttpRequestMessage(HttpMethod.Post, "showtime") { Content = content };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new Exception($"Không thể thêm suất chiếu. Trạng thái: {response.StatusCode}, Body: {body}");
            }
        }

        // Lấy tất cả showtime
        public List<Showtime> GetAllShowtimes()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "showtime");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể lấy danh sách suất chiếu. Trạng thái: {response.StatusCode}");

            var body = response.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("{"))
                return new List<Showtime>();

            return JsonSerializer.Deserialize<List<Showtime>>(body, JsonOptions) ?? new List<Showtime>();
        }

        // Lấy showtime theo ID
        public Showtime? GetShowtimeById(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"showtime/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var body = response.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrWhiteSpace(body) || body.Contains("timestamp"))
                return null;

            return JsonSerializer.Deserialize<Showtime>(body, JsonOptions);
        }

        // Xóa showtime theo ID
        public void DeleteShowtimeById(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"showtime/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể xóa suất chiếu. Trạng thái: {response.StatusCode}");
        }

        // Xóa tất cả showtime
        public void DeleteAllShowtimes()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "showtime");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Không thể xóa tất cả suất chiếu. Trạng thái: {response.StatusCode}");
        }

        // Cập nhật showtime
        public void UpdateShowtime(Guid id, Showtime showtime)
        {
            var content = JsonContent.Create(showtime, options: JsonOptions);
            var request = new HttpRequestMessage(HttpMethod.Put, $"showtime/{id}") { Content = content };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());
            var response = _http.Send(request);

            if (!response.IsSuccessStatusCode)
            {
                var body = response.Content.ReadAsStringAsync().Result;
                throw new Exception($"Không thể cập nhật suất chiếu. Trạng thái: {response.StatusCode}, Body: {body}");
            }
        }
    }
}
