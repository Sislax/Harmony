using Harmony.Domain.Entities;

namespace Harmony.Application.Common.Interfaces.RepositoryInterfaces;

public interface IChannelRepository
{
    void Add(Channel newChannel);
}
