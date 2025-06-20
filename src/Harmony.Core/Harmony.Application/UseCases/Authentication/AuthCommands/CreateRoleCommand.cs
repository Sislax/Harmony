﻿using Harmony.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public record CreateRoleCommand(string RoleName) : IRequest;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<CreateRoleCommandHandler> _logger;

    public CreateRoleCommandHandler(IIdentityService identityService, ILogger<CreateRoleCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        await _identityService.CreateRoleAsync(request.RoleName);

        _logger.LogInformation("Role '{request.RoleName}' created correctly", request.RoleName);
    }
}
