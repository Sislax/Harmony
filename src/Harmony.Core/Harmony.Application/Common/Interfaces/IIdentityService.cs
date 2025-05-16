using Harmony.Application.Models.DTOs;
using Harmony.Application.Models.DTOs.AuthDTOs;

namespace Harmony.Application.Common.Interfaces;

public interface IIdentityService
{
    public Task<RegisterResponseDTO> CreateUserAsync(RegisterRequestDTO registerCredentials);
    public Task<LoginResponseDTO> SignInUserAsync(LoginRequestDTO loginCredentials);
    public Task<UserForTokenDTO> GetUserByEmailAsync(string email);
    public Task<UserForTokenDTO> GetUserByIdAsync(string id);
    public Task CreateRoleAsync(string roleName);
    public Task AssignRoleAsync(string roleName, string userEmail);
}
