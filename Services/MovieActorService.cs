using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Services
{
    public class MovieActorService
    {
        private readonly HttpClient _http;
        private readonly AuthTokenUtil _tokenUtil;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public MovieActorService(HttpClient http, AuthTokenUtil tokenUtil)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:8080/");
            _tokenUtil = tokenUtil;
        }

        public void InsertMovieActors(Guid movieId, List<Guid> actorIds)
        {
            var payload = new MovieActorsRequest(movieId, actorIds);
            var content = JsonContent.Create(payload, options: JsonOptions);

            var request = new HttpRequestMessage(HttpMethod.Post, "MovieActor")
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());

            var response = _http.Send(request);
            var body = response.Content.ReadAsStringAsync().Result;
        }
    }
}
