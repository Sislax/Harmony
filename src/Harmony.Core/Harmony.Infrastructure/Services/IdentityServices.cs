using Harmony.Application.Common.Exceptions;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;
using Harmony.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Harmony.Infrastructure.Services;

public class IdentityServices : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<IdentityServices> _logger;

    public IdentityServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityServices> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<RegisterResponseModel> CreateUserAsync(RegisterRequestModel registerCredential)
    {
        ApplicationUser newUser = new()
        {
            Id = registerCredential.Id,
            FirstName = registerCredential.FirstName,
            LastName = registerCredential.LastName,
            UserName = registerCredential.Username,
            Email = registerCredential.Email
        };

        IdentityResult result = await _userManager.CreateAsync(newUser, registerCredential.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning("User not created with email: {newUser.Email}. Error: {result.Errors.First().Description}", newUser.Email, result.Errors.First().Description);

            throw new UserIdentityException(result.Errors.First().Description);
        }

        _logger.LogInformation("User created with email: {newUser.Email}", newUser.Email);

        return new RegisterResponseModel
        {
            IsSucceded = true,
            UserId = newUser.Id
        };
    }

    public async Task<LoginResponseModel> SignInUserAsync(LoginRequestModel loginCredentials)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(loginCredentials.Email);

        if (user == null)
        {
            _logger.LogError("User not found with email: {loginCredentials.Email}", loginCredentials.Email);

            throw new NotFoundException($"User with email {loginCredentials.Email} does not exist");
        }

        SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName!, loginCredentials.Password, false, false);

        if (!result.Succeeded)
        {
            _logger.LogError("User not signed in with email: {loginCredentials.Email}", loginCredentials.Email);

            throw new UserIdentityException($"An error occurred during the sign in. Try again later...");
        }

        _logger.LogInformation("User signed in with email: {loginCredentialss.Email}", loginCredentials.Email);

        return new LoginResponseModel
        {
            IsSucceded = true,
            UserId = user.Id
        };
    }

    public async Task<UserForTokenDTO> GetUserByEmailAsync(string email)
    {
        ApplicationUser? applicationUser;

        try
        {
            applicationUser = await _userManager.FindByEmailAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error finding user with email: {email}. Exception: {ex}", email, ex);

            throw;
        }

        if (applicationUser == null)
        {
            _logger.LogError("User not found with email: {email}", email);

            throw new NotFoundException($"User not found with email: {email}");
        }

        _logger.LogInformation("User found with email: {email}", email);

        return new UserForTokenDTO
        {
            Id = applicationUser.Id,
            Email = applicationUser.Email!, // Suppressing Warning because we are sure that user.Email is not null
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            Username = applicationUser.UserName! // Suppressing Warning because we are sure that user.UserName is not null
        };
    }

    public async Task<UserForTokenDTO> GetUserByIdAsync(string id)
    {
        ApplicationUser? applicationUser;

        try
        {
            applicationUser = await _userManager.FindByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error finding user with Id: {id}. Exception: {ex}", id, ex);

            throw;
        }

        if (applicationUser == null)
        {
            _logger.LogError("User not found with Id: {id}", id);

            throw new NotFoundException($"User not found with Id: {id}");
        }

        _logger.LogInformation("User found with Id: {id}", id);

        return new UserForTokenDTO
        {
            Id = applicationUser.Id,
            Email = applicationUser.Email!, // Suppressing Warning because we are sure that user.Email is not null
            FirstName = applicationUser.FirstName,
            LastName = applicationUser.LastName,
            Username = applicationUser.UserName! // Suppressing Warning because we are sure that user.UserName is not null
        };
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        try
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));

            _logger.LogInformation("Role '{roleName}' created correctly", roleName);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating the role '{roleName}'. Exception: {ex}", roleName, ex);

            throw;
        }

        return true;
    }

    public async Task<bool> AssignRoleAsync(string roleName, string userEmail)
    {
        ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(userEmail);

        if (applicationUser == null)
        {
            _logger.LogError("User not found with email: {userEmail}", userEmail);

            throw new NotFoundException($"User not found with userEmail: {userEmail}");
        }

        _logger.LogInformation("User found with email: {userEmail}", userEmail);

        IdentityResult result = await _userManager.AddToRoleAsync(applicationUser, roleName);

        if (!result.Succeeded)
        {
            _logger.LogError("Error occurred during the role assignment. Role: {roleName}, User: {applicationUser}", roleName, applicationUser);

            throw new UserIdentityException("Error during the creation of the user.");
        }

        _logger.LogInformation("Role '{roleName}' assigned correctly to the user with  email '{userEmail}'", roleName, userEmail);

        return true;
    }
}