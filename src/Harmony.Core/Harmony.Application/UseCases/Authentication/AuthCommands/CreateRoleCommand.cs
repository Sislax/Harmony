using Harmony.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.Authentication.AuthCommands;

public class CreateRoleCommand : IRequest<bool>
{
    public string RoleName { get; set; }

    public CreateRoleCommand(string roleName)
    {
        RoleName = roleName;
    }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<CreateRoleCommandHandler> _logger;

    public CreateRoleCommandHandler(IIdentityService identityService, ILogger<CreateRoleCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<bool> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        bool result;

        try
        {
            result = await _identityService.CreateRoleAsync(request.RoleName);

            _logger.LogInformation("Role '{request.RoleName}' created correctly", request.RoleName);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating role '{request.RoleName}'. Exception: {ex}", request.RoleName, ex);

            throw;
        }

        return result;
    }
}
