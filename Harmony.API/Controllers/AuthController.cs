using System.Security.Claims;
using Harmony.Application.Models.AuthResponseModels;
using Harmony.Application.Models.DTOs;
using Harmony.Application.UseCases.Commands.AuthCommands;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.API.Controllers;

[Route("api/[controller]/[action]")]
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
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
    {
        RegisterResponseModel result = await _sender.Send(new RegisterCommand(registerDTO));

        if (!result.IsSucceded)
        {
            _logger.LogWarning("User with email {Email} failed to register", registerDTO.Email);

            return BadRequest(result);
        }

        _logger.LogInformation("User with email {Email} has registered", registerDTO.Email);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        LoginResponseModel result = await _sender.Send(new LoginCommand(loginDTO));

        if (!result.IsSucceded)
        {
            _logger.LogWarning("User with email {Email} failed to login", loginDTO.Email);

            return BadRequest(result);
        }
        _logger.LogInformation("User with email {Email} has logged in", loginDTO.Email);

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Claim? userId = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            _logger.LogWarning("User with Id {userId} is not authenticated. Failed to logout", userId);

            return Unauthorized("User is not authenticated");
        }

        LogoutResponseModel result = await _sender.Send(new LogoutCommand(userId));

        if (!result.IsSucceded)
        {
            _logger.LogWarning("User with Id {userId} failed to logout", userId);

            return BadRequest(result);
        }

        _logger.LogInformation("User with Id {userId} has logged out", userId);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshToken refreshToken)
    {
        RefreshTokenResponseModel result = await _sender.Send(new RefreshTokenCommand(refreshToken));

        if (!result.IsSucceded)
        {
            _logger.LogWarning("User with Id {refreshToken.UserId} failed to refresh token", refreshToken.UserId);

            return BadRequest(result);
        }
        _logger.LogInformation("User with Id {refreshToken.UserId} has refreshed token", refreshToken.UserId);

        return Ok(result);
    }
}
