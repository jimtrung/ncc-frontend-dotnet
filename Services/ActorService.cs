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
    public class ActorService
    {
        private readonly HttpClient _http;
        private readonly AuthTokenUtil _tokenUtil;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            // nếu có một biến là firstName thì trong json sẽ là first_name
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public ActorService(HttpClient http, AuthTokenUtil tokenUtil)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:8080/");
            _tokenUtil = tokenUtil;
        }

        public object GetAllActors()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "actor/all");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var token = _tokenUtil.LoadAccessToken();
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
            {
                return new List<Actor>();
            }

            var actors = JsonSerializer.Deserialize<List<Actor>>(body, JsonOptions) ?? new List<Actor>();
            return actors;
        }
    }
}
