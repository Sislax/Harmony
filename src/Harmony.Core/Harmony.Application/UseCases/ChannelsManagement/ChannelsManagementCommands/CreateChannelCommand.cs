using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Application.Models.DTOs.DomainDTOs.ChannelDTOs;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.ChannelsManagement.ChannelsManagementCommands;

public record CreateChannelCommand(CreateChannelRequestDTO CreateChannelDTO) : IRequest;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IServerRepository _serverRepository;
    private readonly IQueryMaterializerFactory _queryMaterializerFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateChannelCommandHandler> _logger;

    public CreateChannelCommandHandler(IChannelRepository channelRepository, IServerRepository serverRepository, IQueryMaterializerFactory queryMaterializerFactory, IUnitOfWork unitOfWork, ILogger<CreateChannelCommandHandler> logger)
    {
        _channelRepository = channelRepository;
        _serverRepository = serverRepository;
        _queryMaterializerFactory = queryMaterializerFactory;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        Guid channelId = Guid.NewGuid();

        List<ServerMember> serverMembers = await _serverRepository.GetAsync(
            filter: s => s.ServerMembers.Any(sm => sm.ServerId == request.CreateChannelDTO.ServerId),
            materializer: _queryMaterializerFactory.ToListAsync<ServerMember>(),
            cancellationToken: cancellationToken
            );

        Channel newChannel = new()
        {
            Id = channelId,
            ChannelName = request.CreateChannelDTO.ChannelName,
            ChannelType = request.CreateChannelDTO.ChannelType,
            ServerId = request.CreateChannelDTO.ServerId,
            ChannelMembers = [.. serverMembers.Select(sm => new ChannelMember
            {
                UserId = sm.UserId,
                ChannelId = channelId
            })]
        };

        _channelRepository.Add(newChannel);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Channel creation successfully completed.");
    }
}