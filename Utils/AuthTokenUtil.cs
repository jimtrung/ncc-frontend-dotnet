namespace Theater_Management_FE.Services;

using System.IO;

/// <summary>
/// Simple file-based token storage so the desktop app does not rely on MAUI Preferences.
/// </summary>
public class AuthTokenUtil
{
    private readonly string _storageFolder;
    private readonly string _accessTokenPath;
    private readonly string _refreshTokenPath;

    private string? _accessToken;
    private string? _refreshToken;

    public AuthTokenUtil()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _storageFolder = Path.Combine(appData, "TheaterManagementFE");
        Directory.CreateDirectory(_storageFolder);

        _accessTokenPath = Path.Combine(_storageFolder, "access.token");
        _refreshTokenPath = Path.Combine(_storageFolder, "refresh.token");
    }

    public void SaveAccessToken(string token)
    {
        _accessToken = token;
        File.WriteAllText(_accessTokenPath, token ?? string.Empty);
    }

    public void SaveRefreshToken(string token)
    {
        _refreshToken = token;
        File.WriteAllText(_refreshTokenPath, token ?? string.Empty);
    }

    public void ClearAccessToken()
    {
        _accessToken = null;
        if (File.Exists(_accessTokenPath))
        {
            File.Delete(_accessTokenPath);
        }
    }

    public void ClearRefreshToken()
    {
        _refreshToken = null;
        if (File.Exists(_refreshTokenPath))
        {
            File.Delete(_refreshTokenPath);
        }
    }

    public string? LoadAccessToken()
    {
        if (!string.IsNullOrEmpty(_accessToken))
            return _accessToken;

        if (File.Exists(_accessTokenPath))
        {
            _accessToken = File.ReadAllText(_accessTokenPath);
            return string.IsNullOrWhiteSpace(_accessToken) ? null : _accessToken;
        }

        return null;
    }

    public string? LoadRefreshToken()
    {
        if (!string.IsNullOrEmpty(_refreshToken))
            return _refreshToken;

        if (File.Exists(_refreshTokenPath))
        {
            _refreshToken = File.ReadAllText(_refreshTokenPath);
            return string.IsNullOrWhiteSpace(_refreshToken) ? null : _refreshToken;
        }

        return null;
    }

    public void SaveTokens(string accessToken, string refreshToken)
    {
        SaveAccessToken(accessToken);
        SaveRefreshToken(refreshToken);
    }
}
