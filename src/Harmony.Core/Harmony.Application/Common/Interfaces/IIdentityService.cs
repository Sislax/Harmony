using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;

namespace Harmony.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<RegisterResponseModel> CreateUserAsync(RegisterRequestModel registerCredentials);
    public Task<LoginResponseModel> SignInUserAsync(LoginRequestModel loginCredentials);
    public Task<UserForTokenDTO> GetUserByEmailAsync(string email);
    public Task<UserForTokenDTO> GetUserByIdAsync(string id);
    public Task<bool> CreateRoleAsync(string roleName);
    public Task<bool> AssignRoleAsync(string roleName, string userEmail);
}
