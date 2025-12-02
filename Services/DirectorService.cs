using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Services
{
    public class DirectorService
    {
        private readonly HttpClient _http;
        private readonly AuthTokenUtil _tokenUtil;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public DirectorService(HttpClient http, AuthTokenUtil tokenUtil)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:8080/");
            _tokenUtil = tokenUtil;
        }

        public object GetAllDirectors()
        {
            var token = _tokenUtil.LoadAccessToken();

            var request = new HttpRequestMessage(HttpMethod.Get, "director/all");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _http.Send(request);
            var body = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(body, JsonOptions);
                return error!;
            }

            if (string.IsNullOrWhiteSpace(body))
                return new List<Director>();

            var directors = JsonSerializer.Deserialize<List<Director>>(body, JsonOptions) ?? new List<Director>();
            return directors;
        }

        public Director? GetDirectorById(Guid id)
        {
            var token = _tokenUtil.LoadAccessToken();

            var request = new HttpRequestMessage(HttpMethod.Get, $"director/{id}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _http.Send(request);
            var body = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(body))
                return null;

            var director = JsonSerializer.Deserialize<Director>(body, JsonOptions);
            return director;
        }
    }
}
