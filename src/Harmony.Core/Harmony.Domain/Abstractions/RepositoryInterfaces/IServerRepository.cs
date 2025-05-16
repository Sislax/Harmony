using Harmony.Domain.Entities;

namespace Harmony.Domain.Abstractions.RepositoryInterfaces;

public interface IServerRepository
{
    void Add(Server server);
    Task<List<Server>> GetServersByUser(string userId);
}
