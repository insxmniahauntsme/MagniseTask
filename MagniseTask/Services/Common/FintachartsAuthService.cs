using System.Text.Json;
using MagniseTask.Data;
using MagniseTask.Interfaces;
using Microsoft.Extensions.Options;

namespace MagniseTask.Services;

public class FintachartsAuthService : IFintachartsAuthService
{
    private readonly HttpClient _httpClient;
    private readonly UserCredentials _userCredentials;
    private readonly AuthDataManager _authDataManager;

    public FintachartsAuthService(IHttpClientFactory httpClientFactory, IOptions<UserCredentials> userCredentials, AuthDataManager authDataManager)
    {
        _httpClient = httpClientFactory.CreateClient("FintachartsClient");
        _userCredentials = userCredentials.Value;
        _authDataManager = authDataManager;
    }

    public async Task EnsureAuthenticatedAsync()
    {
        var authData = _authDataManager.GetAuthData();

        if (authData == null || authData.IsAccessTokenExpired())
        {
            if (authData?.RefreshToken != null && !authData.IsRefreshTokenExpired())
            {
                await RefreshTokenAsync(authData.RefreshToken);
            }
            else
            {
                await AuthenticateAsync();
            }
        }
    }

    public async Task AuthenticateAsync()
    {
        var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "app-cli" },
            { "username", _userCredentials.Username },
            { "password", _userCredentials.Password },
        });

        var response = await _httpClient.PostAsync("/identity/realms/fintatech/protocol/openid-connect/token", requestBody);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<AuthData>(content);

        if (authResponse != null)
        {
            _authDataManager.SetAuthData(authResponse);
        }
    }

    private async Task RefreshTokenAsync(string refreshToken)
    {
        var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", "app-cli" },
            { "refresh_token", refreshToken },
        });

        var response = await _httpClient.PostAsync("/identity/realms/fintatech/protocol/openid-connect/token", requestBody);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var authResponse = JsonSerializer.Deserialize<AuthData>(content);

        if (authResponse != null)
        {
            _authDataManager.SetAuthData(authResponse);
        }
    }
}
