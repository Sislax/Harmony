using Harmony.Application.Common.Interfaces;
using Harmony.Application.Models.DTOs.DomainDTOs.ChannelDTOs;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Harmony.Application.UseCases.ChannelsManagement.ChannelsManagementCommands;

public record CreateChannelCommand(CreateChannelRequestDTO CreateChannelDTO) : IRequest;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IServerRepository _serverRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateChannelCommandHandler> _logger;

    public CreateChannelCommandHandler(IChannelRepository channelRepository, IServerRepository serverRepository, IUnitOfWork unitOfWork, ILogger<CreateChannelCommandHandler> logger)
    {
        _channelRepository = channelRepository;
        _serverRepository = serverRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        Guid channelId = Guid.NewGuid();

        List<ServerMember> serverMembers = await _serverRepository.GetMembersAsync(channelId);

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