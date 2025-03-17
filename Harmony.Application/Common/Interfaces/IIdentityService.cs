using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;

namespace Harmony.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<RegisterResponseModel> CreateUserAsync(RegisterDTO registerCredentials);
    public Task<LoginResponseModel> SignInUserAsync(LoginDTO loginCredentials);
    public Task<UserForTokenDTO> GetUserByEmailAsync(string email);
    public Task<UserForTokenDTO> GetUserByIdAsync(string id);
    public Task<bool> CreateRoleAsync(string roleName);
    public Task<bool> AssignRoleAsync(string roleName, string userEmail);
}
