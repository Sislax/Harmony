using Harmony.Application.Models.DTOs.DomainDTOs;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;

namespace Harmony.Application.UseCases.ServersManagement.ServersManagementQueries;

public record GetServersByUserQuery(string UserId) : IRequest<List<ServerDTO>>;

public class GetServersByUserQueryHandler : IRequestHandler<GetServersByUserQuery, List<ServerDTO>>
{
    private readonly IServerRepository _serverRepository;

    public GetServersByUserQueryHandler(IServerRepository serverRepository)
    {
        _serverRepository = serverRepository;
    }

    public async Task<List<ServerDTO>> Handle(GetServersByUserQuery request, CancellationToken cancellationToken)
    {
        List<Server> serversOfTheUser = await _serverRepository.GetServersByUser(request.UserId);
        return [.. serversOfTheUser.Select(s => new ServerDTO(s.ServerName))];
    }
}
