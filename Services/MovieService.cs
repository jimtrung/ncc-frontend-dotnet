using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;

namespace Theater_Management_FE.Services;

public class MovieService
{
    private readonly HttpClient _http;
    private readonly AuthTokenUtil _tokenUtil;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public MovieService(HttpClient http, AuthTokenUtil tokenUtil)
    {
        _http = http;
        _http.BaseAddress = new Uri("http://localhost:8080/");
        _tokenUtil = tokenUtil;
    }

    public async Task InsertMovie(Movie movie)
    {
        var content = JsonContent.Create(movie, options: JsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Post, "movies") { Content = content };

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
    }

    public async Task DeleteMovieById(Guid id)
    {
        var token = _tokenUtil.LoadAccessToken();
        var request = new HttpRequestMessage(HttpMethod.Delete, $"movies/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _http.SendAsync(request);
    }

    public async Task DeleteAllMovies()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "movies");
        await _http.SendAsync(request);
    }

    public async Task<List<Movie>> GetAllMovies()
    {
        var token = _tokenUtil.LoadAccessToken();
        var request = new HttpRequestMessage(HttpMethod.Get, "movies");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(body))
            return new List<Movie>();

        return JsonSerializer.Deserialize<List<Movie>>(body, JsonOptions) ?? new();
    }

    public async Task<Movie?> GetMovieById(Guid id)
    {
        var token = _tokenUtil.LoadAccessToken();
        var request = new HttpRequestMessage(HttpMethod.Get, $"movies/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(body))
            return null;

        return JsonSerializer.Deserialize<Movie>(body, JsonOptions);
    }

    public async Task UpdateMovie(Guid id, Movie movie)
    {
        var content = JsonContent.Create(movie, options: JsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Put, $"movies/{id}")
        {
            Content = content
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());

        await _http.SendAsync(request);
    }
}
