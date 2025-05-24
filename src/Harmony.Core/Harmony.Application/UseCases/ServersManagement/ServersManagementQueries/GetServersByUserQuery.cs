using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Application.Models.DTOs.DomainDTOs.ServerDTOs;
using MediatR;

namespace Harmony.Application.UseCases.ServersManagement.ServersManagementQueries;

public record GetServersByUserQuery(string UserId) : IRequest<List<GetServersByUserResponseDTO>>;

public class GetServersByUserQueryHandler : IRequestHandler<GetServersByUserQuery, List<GetServersByUserResponseDTO>>
{
    private readonly IServerMemberRepository _serverMemberRepository;
    private readonly IQueryMaterializerFactory _queryMaterializerFactory;

    public GetServersByUserQueryHandler(IServerMemberRepository serverMemberRepository, IQueryMaterializerFactory queryMaterializerFactory)
    {
        _serverMemberRepository = serverMemberRepository;
        _queryMaterializerFactory = queryMaterializerFactory;
    }

    public async Task<List<GetServersByUserResponseDTO>> Handle(GetServersByUserQuery request, CancellationToken cancellationToken)
    {
        return await _serverMemberRepository.GetAsync(
            filter: sm => sm.UserId == request.UserId,
            select: sm => new GetServersByUserResponseDTO(sm.ServerId, sm.Server.ServerName),
            materializer: _queryMaterializerFactory.ToListAsync<GetServersByUserResponseDTO>(),
            cancellationToken: cancellationToken
            );

        //return await _serverRepository.GetAsync(
        //    filter: s => s.ServerMembers.Any(sm => sm.UserId == request.UserId),
        //    select: s => new GetServersByUserResponseDTO(s.Id, s.ServerName),
        //    materializer: _queryMaterializerFactory.ToListAsync<GetServersByUserResponseDTO>(),
        //    cancellationToken: cancellationToken
        //    );
    }
}
