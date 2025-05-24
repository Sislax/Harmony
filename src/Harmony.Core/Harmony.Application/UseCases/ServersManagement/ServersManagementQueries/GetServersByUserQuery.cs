using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Application.Models.DTOs.DomainDTOs.ServerDTOs;
using Harmony.Domain.Entities;
using MediatR;

namespace Harmony.Application.UseCases.ServersManagement.ServersManagementQueries;

public record GetServersByUserQuery(string UserId) : IRequest<List<GetServersByUserResponseDTO>>;

public class GetServersByUserQueryHandler : IRequestHandler<GetServersByUserQuery, List<GetServersByUserResponseDTO>>
{
    private readonly IServerRepository _serverRepository;
    private readonly IQueryMaterializerFactory _queryMaterializerFactory;

    public GetServersByUserQueryHandler(IServerRepository serverRepository, IQueryMaterializerFactory queryMaterializerFactory)
    {
        _serverRepository = serverRepository;
        _queryMaterializerFactory = queryMaterializerFactory;
    }

    public async Task<List<GetServersByUserResponseDTO>> Handle(GetServersByUserQuery request, CancellationToken cancellationToken)
    {
        var serversOfUser = await _serverRepository.GetAsync(
            filter: s => s.ServerMembers.Any(sm => sm.UserId == request.UserId),
            materializer: _queryMaterializerFactory.ToListAsync<Server>(),
            cancellationToken: cancellationToken
            );
        List<Server> serversOfTheUser = await _serverRepository.GetServersByUserAsync(request.UserId);
        return [.. serversOfTheUser.Select(s => new GetServersByUserResponseDTO(s.Id, s.ServerName))];
    }
}
