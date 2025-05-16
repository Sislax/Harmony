using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories;

public class ServerRepository : IServerRepository
{
    public readonly ApplicationDbContext _context;

    public ServerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Server server)
    {
        _context.Servers.Add(server);
    }

    public async Task<List<Server>> GetServersByUser(string userId)
    {
        return await _context.ServerMembers.Where(sm => sm.UserId == userId)
                .Select(sm => sm.Server)
                .ToListAsync();
    }
}
