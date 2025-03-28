using Harmony.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Commands.AuthCommands;

public class AssignRoleCommand : IRequest<bool>
{
    public string RoleName { get; set; }
    public string UserEmail { get; set; }

    public AssignRoleCommand(string roleName, string userEmail)
    {
        RoleName = roleName;
        UserEmail = userEmail;
    }
}

public class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<AssignRoleCommandHandler> _logger;

    public AssignRoleCommandHandler(IIdentityService identityService, ILogger<AssignRoleCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<bool> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        bool result;

        try
        {
            result = await _identityService.AssignRoleAsync(request.RoleName, request.UserEmail);

            _logger.LogInformation("Role '{request.RoleName}' assigned correctly to the user with email {request.UserEmail}", request.RoleName, request.UserEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error assigning role '{request.RoleName}' to the user with email '{request.UserEmail}'. Exception: {ex}", request.RoleName, request.UserEmail, ex);

            throw;
        }

        return result;
    }
}
