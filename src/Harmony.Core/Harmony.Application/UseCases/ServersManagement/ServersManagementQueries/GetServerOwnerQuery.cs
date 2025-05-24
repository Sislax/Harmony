using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.ServersManagement.ServersManagementQueries;

public record GetServerOwnerQuery(Guid ServerId) : IRequest<Guid>;

public class GetServerOwnerQueryHandler : IRequestHandler<GetServerOwnerQuery, Guid>
{
    private readonly IServerRepository _serverRepository;
    private readonly ILogger<GetServerOwnerQueryHandler> _logger;

    public GetServerOwnerQueryHandler(IServerRepository serverRepository, ILogger<GetServerOwnerQueryHandler> logger)
    {
        _serverRepository = serverRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(GetServerOwnerQuery request, CancellationToken cancellationToken)
    {
        
    }
}
