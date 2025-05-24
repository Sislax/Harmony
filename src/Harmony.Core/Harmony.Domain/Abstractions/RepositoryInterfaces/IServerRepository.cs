using Harmony.Domain.Entities;

namespace Harmony.Domain.Abstractions.RepositoryInterfaces;

public interface IServerRepository
{
    void Add(Server server);
    Task<Server> GetServerByIdAsync(Guid id);
    Task<List<Server>> GetServersByUserAsync(string userId);
    Task<List<ServerMember>> GetMembersAsync(Guid serverId);
}
