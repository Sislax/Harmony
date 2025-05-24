using System.Security.Claims;
using Harmony.Application.Models.DTOs.DomainDTOs.ServerDTOs;
using Harmony.Application.UseCases.ServersManagement.ServersManagementCommands;
using Harmony.Application.UseCases.ServersManagement.ServersManagementQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.API.Controllers;

public class ServerController : ControllerBase
{
    public readonly ISender _sender;
    public readonly ILogger<ServerController> _logger;

    public ServerController(ISender sender, ILogger<ServerController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [Authorize]
    [HttpPost("createServer")]
    public async Task<IActionResult> CreateServer(string serverName)
    {
        Claim? userId = User.FindFirst(ClaimTypes.NameIdentifier);

        if(userId == null)
        {
            _logger.LogError("Error during the server creation: User Identifier not found.");

            return Unauthorized("User is not authenticated.");
        }

        await _sender.Send(new CreateServerCommand(new CreateServerRequestDTO(serverName, userId.ToString())));

        return Ok();
    }

    [Authorize]
    [HttpGet("getServersByUser")]
    public async Task<IActionResult> GetServersByUser()
    {
        Claim? userId = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            _logger.LogError("Error during the server creation: User Identifier not found.");

            return Unauthorized("User is not authenticated.");
        }

        List<GetServersByUserResponseDTO> serversOfTheUser = await _sender.Send(new GetServersByUserQuery(userId.ToString()));

        return Ok(serversOfTheUser);
    }

}
