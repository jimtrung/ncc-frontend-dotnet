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
    public class MovieService
    {
        private readonly HttpClient _http;
        private readonly AuthTokenUtil _tokenUtil;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };

        public MovieService(HttpClient http, AuthTokenUtil tokenUtil)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:8080/");
            _tokenUtil = tokenUtil;
        }

        public void InsertMovie(Movie movie)
        {
            var content = JsonContent.Create(movie, options: JsonOptions);
            var request = new HttpRequestMessage(HttpMethod.Post, "Movie") { Content = content };
            var response = _http.Send(request);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = response.Content.ReadAsStringAsync().Result;
                throw new Exception($"Failed to insert movie. Status: {response.StatusCode}, Body: {errorBody}");
            }
        }

        public void DeleteMovieById(Guid id)
        {
            var token = _tokenUtil.LoadAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"Movie/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _http.Send(request);
             if (!response.IsSuccessStatusCode)
            {
                 throw new Exception($"Failed to delete movie. Status: {response.StatusCode}");
            }
        }

        public void DeleteAllMovies()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "Movie");
            var response = _http.Send(request);
             if (!response.IsSuccessStatusCode)
            {
                 throw new Exception($"Failed to delete all movies. Status: {response.StatusCode}");
            }
        }

        public List<Movie> GetAllMovies()
        {
            var token = _tokenUtil.LoadAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "Movie");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _http.Send(request);
            if (!response.IsSuccessStatusCode)
            {
                 throw new Exception($"Failed to get movies. Status: {response.StatusCode}");
            }
            var body = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(body))
                return new List<Movie>();

            return JsonSerializer.Deserialize<List<Movie>>(body, JsonOptions) ?? new List<Movie>();
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            var token = _tokenUtil.LoadAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "Movie");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                 throw new Exception($"Failed to get movies. Status: {response.StatusCode}");
            }
            var body = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(body))
                return new List<Movie>();

            return JsonSerializer.Deserialize<List<Movie>>(body, JsonOptions) ?? new List<Movie>();
        }

        public Movie? GetMovieById(Guid id)
        {
            var token = _tokenUtil.LoadAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Get, $"Movie/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _http.Send(request);
            if (!response.IsSuccessStatusCode)
            {
                 throw new Exception($"Failed to get movie by id. Status: {response.StatusCode}");
            }
            var body = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(body))
                return null;

            return JsonSerializer.Deserialize<Movie>(body, JsonOptions);
        }

        public void UpdateMovie(Guid id, Movie movie)
        {
            var content = JsonContent.Create(movie, options: JsonOptions);
            var request = new HttpRequestMessage(HttpMethod.Put, $"Movie/{id}") { Content = content };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());
            var response = _http.Send(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update movie. Status: {response.StatusCode}");
            }
        }
    }
}
