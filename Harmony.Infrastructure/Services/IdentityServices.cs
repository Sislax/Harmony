using Harmony.Application.Common.Exceptions;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.DTOs;
using Harmony.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Harmony.Infrastructure.Services
{
    public class IdentityServices : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateUserAsync(RegisterDTO registerCredential)
        {
            ApplicationUser newUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = registerCredential.FirstName,
                LastName = registerCredential.LastName,
                UserName = registerCredential.Username,
                Email = registerCredential.Email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, registerCredential.Password);

            return result.Succeeded;
        }

        public async Task<bool> SignInUserAsync(LoginDTO loginCredentialss)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(loginCredentialss.Email, loginCredentialss.Password, false, false);
            
            return result.Succeeded;
        }

        public async Task<UserForTokenDTO> GetUserByEmail(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new NotFoundException($"User not found with email: {email}");
            }

            #pragma warning disable CS8601 // Possible null reference assignment.
            return new UserForTokenDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName
            };
            #pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
}
