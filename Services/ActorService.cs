using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;

namespace MyApp.Services;

public class ActorService
{
    private readonly HttpClient _http;
    private readonly AuthTokenUtil _tokenUtil;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ActorService(HttpClient http, AuthTokenUtil tokenUtil)
    {
        _http = http;
        _http.BaseAddress = new Uri("http://localhost:8080/");
        _tokenUtil = tokenUtil;
    }

    public async Task<object> GetAllActors()
    {
        Console.WriteLine("[DEBUG] Loading actors...");
        var token = _tokenUtil.LoadAccessToken();
        Console.WriteLine($"[DEBUG] Using token: {token}");

        var request = new HttpRequestMessage(HttpMethod.Get, "actor/all");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _http.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"[DEBUG] - getAllActors - Response status code: {response.StatusCode}");
        Console.WriteLine($"[DEBUG] - getAllActors - Response body: {body}");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("[DEBUG] - getAllActors - Failed to fetch actors.");
            var error = JsonSerializer.Deserialize<ErrorResponse>(body, JsonOptions);
            return error!;
        }

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("[DEBUG] - getAllActors - Empty list received.");
            return new List<Actor>();
        }

        var actors = JsonSerializer.Deserialize<List<Actor>>(body, JsonOptions) ?? new List<Actor>();
        Console.WriteLine($"[DEBUG] Loaded {actors.Count} actors.");
        return actors;
    }
}
