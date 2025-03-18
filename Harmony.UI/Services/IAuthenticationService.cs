using Harmony.UI.Models.AuthenticationModels;

namespace Harmony.UI.Services
{
    public interface IAuthenticationService
    {
        ValueTask<string?> GetJwtAsync();
        Task<bool> LoginAsync(LoginRequestModel loginRequest);
        Task LogoutAsync();
        Task<bool> RefreshAsync();
    }
}