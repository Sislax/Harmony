using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Harmony.UI.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private const string JWT_KEY = nameof(JWT_KEY);
        private const string REFRESH_KEY = nameof(REFRESH_KEY);

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string? token = await _localStorageService.GetItemAsync<string>(JWT_KEY);

                if (token == null)
                {
                    // Anonymous User
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                ClaimsIdentity claimsIdentity = ParseClaimsFromJwt(token);

                return new AuthenticationState(new ClaimsPrincipal(claimsIdentity));
            }
            catch (InvalidOperationException)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void NotifyAuthenticationStateHasChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private ClaimsIdentity ParseClaimsFromJwt(string token)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(token);

            return new ClaimsIdentity(jwt.Claims);
        }
    }
}
