using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;

namespace Harmony.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        public Task<bool> CreateUserAsync(RegisterDTO registerCredentials);
        public Task<bool> SignInUserAsync(LoginDTO loginCredentials);
        public Task<UserForTokenDTO> GetUserByEmail(string email);
    }
}
