using Harmony.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public class AssignRoleCommand : IRequest
{
    public string RoleName { get; set; }
    public string UserEmail { get; set; }

    public AssignRoleCommand(string roleName, string userEmail)
    {
        RoleName = roleName;
        UserEmail = userEmail;
    }
}

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<AssignRoleCommandHandler> _logger;

    public AssignRoleCommandHandler(IIdentityService identityService, ILogger<AssignRoleCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        await _identityService.AssignRoleAsync(request.RoleName, request.UserEmail);

        _logger.LogInformation("Role '{request.RoleName}' assigned correctly to the user with email {request.UserEmail}", request.RoleName, request.UserEmail);
    }
}