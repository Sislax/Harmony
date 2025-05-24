using Harmony.Domain.Entities;

namespace Harmony.Application.Common.Interfaces.RepositoryInterfaces;

public interface IServerRepository : IGenericRepository<Server>
{
    Task<List<Server>> GetServersByUserAsync(string userId);
    Task<List<ServerMember>> GetMembersAsync(Guid serverId);
}
