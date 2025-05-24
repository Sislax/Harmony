using Harmony.Domain.Entities;

namespace Harmony.Domain.Abstractions.RepositoryInterfaces;

public interface IChannelRepository
{
    void Add(Channel newChannel);
}
