namespace Theater_Management_FE.Services;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Utils;

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

    public object SignUp(User user)
    {
        var body = new
        {
            username = user.Username,
            email = user.Email,
            password = user.Password
        };

        HttpResponseMessage response;
        try
        {
            response = _httpClient.PostAsJsonAsync("auth/signup", body, JsonOptions).Result;
        }
        catch (Exception ex)
        {
            return new ErrorResponse(DateTime.UtcNow, 500, "Yêu cầu đăng ký thất bại", ex.Message, "/auth/signup");
        }

        if (!response.IsSuccessStatusCode)
        {
            try
            {
                var error = response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions).Result;
                if (error != null)
                {
                    return error with { Message = !string.IsNullOrWhiteSpace(error.Error) ? error.Error : "Đã xảy ra lỗi không xác định" };
                }
            } catch {} 

            return new ErrorResponse(DateTime.UtcNow, (int)response.StatusCode, "Đăng ký thất bại", "", "/auth/signup");
        }

        return response.Content.ReadAsStringAsync().Result;
    }

    public object SignIn(User user)
    {
        var body = new
        {
            username = user.Username,
            password = user.Password
        };

        HttpResponseMessage response;
        try
        {
            response = _httpClient.PostAsJsonAsync("auth/signin", body, JsonOptions).Result;
        }
        catch (Exception ex)
        {
            return new ErrorResponse(DateTime.UtcNow, 500, "Yêu cầu đăng nhập thất bại", ex.Message, "/auth/signin");
        }

        var raw = response.Content.ReadAsStringAsync().Result ?? "";

        if (!response.IsSuccessStatusCode)
        {
            try
            {
                var error = response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions).Result;
                if (error != null)
                {
                    return error with { Message = !string.IsNullOrWhiteSpace(error.Error) ? error.Error : "Đã xảy ra lỗi không xác định" };
                }
            }
            catch {}

            return new ErrorResponse(DateTime.UtcNow, (int)response.StatusCode, "Đăng nhập thất bại", !string.IsNullOrWhiteSpace(raw) ? raw : "Lỗi máy chủ không xác định", "/auth/signin");
        }

        try
        {
            var token = response.Content.ReadFromJsonAsync<TokenPair>(JsonOptions).Result;
            if (token == null)
            {
                return new ErrorResponse(DateTime.UtcNow, 500, "Phản hồi token không hợp lệ", "Dữ liệu token rỗng", "/auth/signin");
            }

            return token;
        }
        catch (Exception ex)
        {
            return new ErrorResponse(DateTime.UtcNow, 500, "Không thể đọc dữ liệu token", $"Không thể đọc dữ liệu token: {ex.Message}. Raw response: {raw}", "/auth/signin");
        }
    }

    public object GetUser()
    {
        var token = _authTokenUtil.LoadAccessToken();

        if (string.IsNullOrEmpty(token))
        {
            return new ErrorResponse(DateTime.UtcNow, 401, "Không được phép", "Không có token truy cập", "/user/");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, "user/");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = _httpClient.SendAsync(request).Result;

        if (!response.IsSuccessStatusCode)
        {
            var error = response.Content.ReadFromJsonAsync<ErrorResponse>(JsonOptions).Result;
            return error!;
        }

        var user = response.Content.ReadFromJsonAsync<User>(JsonOptions).Result;
        return user ?? new User();
    }

    public string? Refresh()
    {
        var body = new
        {
            refresh_token = _authTokenUtil.LoadRefreshToken()
        };

        var response = _httpClient.PostAsJsonAsync("auth/refresh", body, JsonOptions).Result;
        if (!response.IsSuccessStatusCode) return null;
        
        return response.Content.ReadAsStringAsync().Result;
    }
}
