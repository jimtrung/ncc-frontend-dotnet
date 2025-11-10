namespace Theater_Management_FE.Services;

using Microsoft.Maui.Storage;

public class AuthTokenUtil
{
    private string? _accessToken;
    private const string KeyRefreshToken = "refreshToken";

    public void SaveAccessToken(string token)
    {
        _accessToken = token;
    }

    public void SaveRefreshToken(string token)
    {
        Preferences.Set(KeyRefreshToken, token);
    }

    public void ClearAccessToken()
    {
        _accessToken = null;
    }

    public void ClearRefreshToken()
    {
        Preferences.Remove(KeyRefreshToken);
    }

    public string? LoadAccessToken()
    {
        return _accessToken;
    }

    public string? LoadRefreshToken()
    {
        return Preferences.Get(KeyRefreshToken, null);
    }

    public void SaveTokens(string accessToken, string refreshToken)
    {
        SaveAccessToken(accessToken);
        SaveRefreshToken(refreshToken);
    }
}
