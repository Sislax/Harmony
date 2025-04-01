using Harmony.UI.Models.AuthenticationModels;

namespace Harmony.UI.Services
{
    public interface IAuthenticationService
    {
        ValueTask<string?> GetJwtAsync();
        Task<bool> LoginAsync(LoginRequestModel loginRequest);
        Task<bool> RegisterAsync(RegisterRequestModel registerRequest);
        Task LogoutAsync();
        Task<bool> RefreshAsync();
    }
}