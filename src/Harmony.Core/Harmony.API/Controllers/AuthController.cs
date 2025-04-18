﻿using System.Security.Claims;
using Harmony.Application.Models.AuthResponseModels;
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
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerDTO)
    {
        RegisterResponseModel result = await _sender.Send(new RegisterCommand(registerDTO));

        _logger.LogInformation("User with email {Email} has registered", registerDTO.Email);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel loginDTO)
    {
        LoginResponseModel result = await _sender.Send(new LoginCommand(loginDTO));

        _logger.LogInformation("User with email {Email} has logged in", loginDTO.Email);

        if (!result.IsSucceded)
        {
            _logger.LogWarning("User with email {Email} failed to login", loginDTO.Email);

            return Problem(statusCode: 500, title: "An unknown error occurred during the login process.");
        }

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

        bool result;

        try
        {
            result = await _sender.Send(new LogoutCommand(userId));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while logging. Exception: {ex}", ex);

            return StatusCode(500, "Ops... Something went wrong. Logout failed.");
        }

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
        RefreshTokenResponseModel result;

        try
        {
            result = await _sender.Send(new RefreshTokenCommand(refreshToken));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while refresh the token. Exception: {ex}", ex);

            return StatusCode(500, "Ops... Something went wrong. Refresh failed.");
        }

        if (!result.IsSucceded)
        {
            _logger.LogWarning("Failed to refresh the token '{refreshToken}'", refreshToken);

            return BadRequest(result);
        }
        _logger.LogInformation("Successfully refreshed with token '{refreshToken}'", refreshToken);

        return Ok(result);
    }
}
