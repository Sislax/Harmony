using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;

namespace Harmony.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<RegisterResponseModel> CreateUserAsync(RegisterDTO registerCredentials);
    public Task<LoginResponseModel> SignInUserAsync(LoginDTO loginCredentials);
    public Task<UserForTokenDTO> GetUserByEmail(string email);
    public Task<UserForTokenDTO> GetUserById(string id);
}
