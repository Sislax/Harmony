using System.Security.Claims;
using System.Threading.Tasks;
using Harmony.Application.Models.DTOs.AuthDTOs;
using Harmony.Application.UseCases.Authentication.AuthCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ISender sender, ILogger<AuthController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
    {
        RegisterResponseDTO result = await _sender.Send(new RegisterCommand(registerDTO));

        _logger.LogInformation("User with email {Email} has registered", registerDTO.Email);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
    {
        LoginResponseDTO result = await _sender.Send(new LoginCommand(loginDTO));

        _logger.LogInformation("User with email {Email} has logged in", loginDTO.Email);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        Claim? userId = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            _logger.LogWarning("User with Id {userId} is not authenticated. Failed to logout", userId);

            return Unauthorized("User is not authenticated");
        }

        bool result = await _sender.Send(new LogoutCommand(userId));

        if (!result)
        {
            _logger.LogWarning("User with Id {userId} failed to logout", userId);

            return BadRequest(result);
        }

        _logger.LogInformation("User with Id {userId} has logged out", userId);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        RefreshTokenResponseDTO result = await _sender.Send(new RefreshTokenCommand(refreshToken));

        if (!result.IsSucceded)
        {
            _logger.LogWarning("Failed to refresh the token '{refreshToken}'", refreshToken);

            return BadRequest(result);
        }
        _logger.LogInformation("Successfully refreshed with token '{refreshToken}'", refreshToken);

        return Ok(result);
    }
}
