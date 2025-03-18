using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Hramony.UI.SharedServices;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;

    public CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ProtectedBrowserStorageResult<string> token = await _localStorage.GetAsync<string>("AccessToken");

        if (string.IsNullOrEmpty(token.Value))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    private IEnumerable<Claim>? ParseClaimFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];

        byte[] jsonBytes = Convert.FromBase64String(payload);

        Dictionary<string, object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes) ?? throw new NullReferenceException();

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }
}
