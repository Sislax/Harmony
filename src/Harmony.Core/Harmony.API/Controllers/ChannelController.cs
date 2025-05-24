using System.Security.Claims;
using Harmony.Application.Models.DTOs.DomainDTOs.ChannelDTOs;
using Harmony.Application.UseCases.ChannelsManagement.ChannelsManagementCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.API.Controllers;

public class ChannelController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<ChannelController> _logger;

    public ChannelController(ISender sender, ILogger<ChannelController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("createChannel")]
    public async Task<IActionResult> CreateChannel(CreateChannelRequestDTO createChannelRequestDTO)
    {
        Claim? userId = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            _logger.LogError("Error during the server creation: User Identifier not found.");

            return Unauthorized("User is not authenticated.");
        }

        await _sender.Send(new CreateChannelCommand(createChannelRequestDTO));

        return Ok();
    }
}
