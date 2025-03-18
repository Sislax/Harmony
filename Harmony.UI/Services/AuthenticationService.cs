using System.Net.Http.Json;
using Blazored.LocalStorage;
using Harmony.UI.Models.AuthenticationModels;

namespace Harmony.UI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _factory;
    private readonly ILocalStorageService _localStorageService;
    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
    private const string JWT_KEY = nameof(JWT_KEY);
    private const string REFRESH_KEY = nameof(REFRESH_KEY);

    public AuthenticationService(IHttpClientFactory factory, ILocalStorageService localStorageService, CustomAuthenticationStateProvider customAuthenticationStateProvider)
    {
        _factory = factory;
        _localStorageService = localStorageService;
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
    }

    //public async Task<RegisterResponseModel> RegisterAsync(RegisterRequestModel)
    //{
    //
    //}

    public async Task<bool> LoginAsync(LoginRequestModel loginRequest)
    {
        HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
            .PostAsync("api/auth/login", JsonContent.Create(loginRequest));

        if (response.IsSuccessStatusCode)
        {
            LoginResponseModel? content = await response.Content.ReadFromJsonAsync<LoginResponseModel>();

            if (content == null)
            {
                return false;
            }

            await _localStorageService.SetItemAsync(JWT_KEY, content.Token);
            await _localStorageService.SetItemAsync(REFRESH_KEY, content.RefreshToken);

            _customAuthenticationStateProvider.NotifyAuthenticationStateHasChanged();
        }

        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync()
    {
        HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
            .GetAsync("api/auth/logout");

        if (response.IsSuccessStatusCode)
        {
            await _localStorageService.RemoveItemAsync(JWT_KEY);
            await _localStorageService.RemoveItemAsync(REFRESH_KEY);

            _customAuthenticationStateProvider.NotifyAuthenticationStateHasChanged();
        }
    }

    public async Task<bool> RefreshAsync()
    {
        string? refreshToken = await _localStorageService.GetItemAsync<string>(REFRESH_KEY);

        if (refreshToken != null)
        {
            HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
            .PostAsync("api/auth/refresh", JsonContent.Create(refreshToken));

            if (response.IsSuccessStatusCode)
            {
                RefreshTokenResponseModel content = await response.Content.ReadFromJsonAsync<RefreshTokenResponseModel>() ?? throw new InvalidDataException();
            
                if(content != null)
                {
                    await _localStorageService.SetItemAsync(JWT_KEY, content.AccessToken);
                    await _localStorageService.SetItemAsync(REFRESH_KEY, content.RefreshToken);

                    _customAuthenticationStateProvider.NotifyAuthenticationStateHasChanged();

                    return true;
                }
            }
        }

        return false;

        //if (refreshToken == null)
        //{
        //    await LogoutAsync();
        //
        //    return false;
        //}
        //
        //HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
        //    .PostAsync("api/auth/refresh", JsonContent.Create(refreshToken));
        //
        //if (!response.IsSuccessStatusCode)
        //{
        //    await LogoutAsync();
        //
        //    return false;
        //}
        //
        //RefreshTokenResponseModel? content = await response.Content.ReadFromJsonAsync<RefreshTokenResponseModel>();
        //
        //if(content == null)
        //{
        //    await LogoutAsync();
        //
        //    return false;
        //}
        //
        //await _localStorageService.SetItemAsync(JWT_KEY, content.AccessToken);
        //await _localStorageService.SetItemAsync(REFRESH_KEY, content.RefreshToken);
        //
        //_customAuthenticationStateProvider.NotifyAuthenticationStateHasChanged();
        //
        //return true;
    }

    public async ValueTask<string?> GetJwtAsync()
    {
        string? jwt = await _localStorageService.GetItemAsync<string>(JWT_KEY);

        return jwt;
    }
}
