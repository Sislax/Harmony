using Harmony.Domain.Enums;

namespace Harmony.Application.Models.DTOs.DomainDTOs.ChannelDTOs;

public class CreateChannelRequestDTO
{
    public string ChannelName { get; set; }
    public ChannelType ChannelType { get; set; }
    public Guid ServerId { get; set; }

    public CreateChannelRequestDTO(string channelName, ChannelType channelType, Guid serverId)
    {
        ChannelName = channelName;
        ChannelType = channelType;
        ServerId = serverId;
    }
}
