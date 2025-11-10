namespace Theater_Management_BE.src.Services;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthTokenUtil _authTokenUtil;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
        Converters = { new JsonStringEnumConverter() }
    };

    public AuthService(HttpClient httpClient, AuthTokenUtil authTokenUtil)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:8080/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        _authTokenUtil = authTokenUtil;
    }

    public async Task<object> SignUp(User user)
    {
        var body = new
        {
            username = user.Username,
            email = user.Email,
            phone_number = user.PhoneNumber,
            password = user.Password
        };

        var response = await _httpClient.PostAsJsonAsync("auth/signup", body, JsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);
            return error!;
        }

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<object> SignIn(User user)
    {
        var body = new
        {
            username = user.Username,
            password = user.Password
        };

        var response = await _httpClient.PostAsJsonAsync("auth/signin", body, JsonOptions);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);
            return error!;
        }

        return await response.Content.ReadFromJsonAsync<TokenPair>(JsonOptions) ?? new TokenPair("", "");
    }

    public async Task<object> GetUser()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "user/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authTokenUtil.LoadAccessToken());
        
        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions);
            return error!;
        }

        return await response.Content.ReadFromJsonAsync<User>(JsonOptions) ?? new User();
    }

    public async Task<string> Refresh()
    {
        var body = new
        {
            refresh_token = _authTokenUtil.LoadRefreshToken()
        };

        var response = await _httpClient.PostAsJsonAsync("auth/refresh", body, JsonOptions);
        return await response.Content.ReadAsStringAsync();
    }
}
