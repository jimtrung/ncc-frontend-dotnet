using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;

namespace Theater_Management_FE.Services;

public class AuditoriumService
{
    private readonly HttpClient _http;
    private readonly AuthTokenUtil _tokenUtil;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public AuditoriumService(HttpClient http, AuthTokenUtil tokenUtil)
    {
        _http = http;
        _http.BaseAddress = new Uri("http://localhost:8080/");
        _tokenUtil = tokenUtil;
    }

    public async Task InsertAuditorium(Auditorium auditorium)
    {
        var content = JsonContent.Create(auditorium, options: JsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Post, "auditoriums") { Content = content };

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
    }

    public async Task<List<Auditorium>> GetAllAuditoriums()
    {
        var token = _tokenUtil.LoadAccessToken();
        var request = new HttpRequestMessage(HttpMethod.Get, "auditoriums");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(body) || body.TrimStart().StartsWith("{"))
            return new List<Auditorium>();

        return JsonSerializer.Deserialize<List<Auditorium>>(body, JsonOptions) ?? new();
    }

    public async Task<Auditorium?> GetAuditoriumById(Guid id)
    {
        var token = _tokenUtil.LoadAccessToken();
        var request = new HttpRequestMessage(HttpMethod.Get, $"auditoriums/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(body) || body.Contains("timestamp"))
            return null;

        return JsonSerializer.Deserialize<Auditorium>(body, JsonOptions);
    }

    public async Task DeleteAuditoriumById(Guid id)
    {
        var token = _tokenUtil.LoadAccessToken();
        var request = new HttpRequestMessage(HttpMethod.Delete, $"auditoriums/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await _http.SendAsync(request);
    }

    public async Task DeleteAllAuditoriums()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "auditoriums");
        await _http.SendAsync(request);
    }

    public async Task UpdateAuditorium(Guid id, Auditorium auditorium)
    {
        var content = JsonContent.Create(auditorium, options: JsonOptions);
        var request = new HttpRequestMessage(HttpMethod.Put, $"auditoriums/{id}")
        {
            Content = content
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenUtil.LoadAccessToken());

        await _http.SendAsync(request);
    }
}
