using Harmony.UI.Models.AuthenticationModels;

namespace Harmony.UI.Services
{
    public interface IAuthenticationService
    {
        event Action<string?>? LoginChange;
        ValueTask<string> GetJwtAsync();
        Task<bool> LoginAsync(LoginRequestModel loginRequest);
        Task LogoutAsync();
        Task<bool> RefreshAsync();
    }
}