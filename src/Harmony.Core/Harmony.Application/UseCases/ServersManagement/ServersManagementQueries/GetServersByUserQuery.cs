using Harmony.Application.Models.DTOs.DomainDTOs.ServerDTOs;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;

namespace Harmony.Application.UseCases.ServersManagement.ServersManagementQueries;

public record GetServersByUserQuery(string UserId) : IRequest<List<GetServersByUserResponseDTO>>;

public class GetServersByUserQueryHandler : IRequestHandler<GetServersByUserQuery, List<GetServersByUserResponseDTO>>
{
    private readonly IServerRepository _serverRepository;

    public GetServersByUserQueryHandler(IServerRepository serverRepository)
    {
        _serverRepository = serverRepository;
    }

    public async Task<List<GetServersByUserResponseDTO>> Handle(GetServersByUserQuery request, CancellationToken cancellationToken)
    {
        List<Server> serversOfTheUser = await _serverRepository.GetServersByUserAsync(request.UserId);
        return [.. serversOfTheUser.Select(s => new GetServersByUserResponseDTO(s.Id, s.ServerName))];
    }
}
