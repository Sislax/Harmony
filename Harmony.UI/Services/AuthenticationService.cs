using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.SessionStorage;
using Harmony.UI.Models.AuthenticationModels;

namespace Harmony.UI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _factory;
    private readonly ISessionStorageService _sessionStorageService;
    private string? jwtCache;
    private const string JWT_KEY = nameof(JWT_KEY);
    private const string REFRESH_KEY = nameof(REFRESH_KEY);
    public event Action<string?>? LoginChange;

    public AuthenticationService(IHttpClientFactory factory, ISessionStorageService sessionStorageService)
    {
        _factory = factory;
        _sessionStorageService = sessionStorageService;
    }

    //public async Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel)
    //{
    //
    //}

    public async Task<bool> LoginAsync(LoginRequestModel loginRequest)
    {
        HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
            .PostAsync("api/auth/login", JsonContent.Create(loginRequest));

        if (!response.IsSuccessStatusCode)
        {
            throw new UnauthorizedAccessException("Login Failed.");
        }

        LoginResponseModel? content = await response.Content.ReadFromJsonAsync<LoginResponseModel>() ?? throw new InvalidDataException();

        await _sessionStorageService.SetItemAsync(JWT_KEY, content.Token);
        await _sessionStorageService.SetItemAsync(REFRESH_KEY, content.RefreshToken);

        LoginChange?.Invoke(GetUserName(content.Token!));

        return content.IsSucceded;
    }

    public async Task LogoutAsync()
    {
        HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
            .GetAsync("api/auth/logout");

        if (!response.IsSuccessStatusCode)
        {
            throw new UnauthorizedAccessException("Logout Failed.");
        }

        await _sessionStorageService.RemoveItemAsync(JWT_KEY);
        await _sessionStorageService.RemoveItemAsync(REFRESH_KEY);

        jwtCache = null;

        LoginChange?.Invoke(null);
    }

    public async Task<bool> RefreshAsync()
    {
        string refreshToken = await _sessionStorageService.GetItemAsync<string>(REFRESH_KEY);

        HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
            .PostAsync("api/auth/refresh", JsonContent.Create(refreshToken));

        if (!response.IsSuccessStatusCode)
        {
            await LogoutAsync();

            return false;
        }

        RefreshTokenResponseModel content = await response.Content.ReadFromJsonAsync<RefreshTokenResponseModel>() ?? throw new InvalidDataException();

        await _sessionStorageService.SetItemAsync(JWT_KEY, content.AccessToken);
        await _sessionStorageService.SetItemAsync(REFRESH_KEY, content.RefreshToken);

        return true;
    }

    public async ValueTask<string> GetJwtAsync()
    {
        if (string.IsNullOrEmpty(jwtCache))
        {
            jwtCache = await _sessionStorageService.GetItemAsync<string>(JWT_KEY);
        }

        return jwtCache;
    }

    private static string GetUserName(string token)
    {
        JwtSecurityToken jwt = new JwtSecurityToken(token);

        return jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;
    }
}
