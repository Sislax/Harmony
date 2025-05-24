using System.Linq.Expressions;
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

    public async Task<Server> GetServerByIdAsync(Guid id)
    {
        return await _context.Servers.SingleAsync(s => s.Id == id);
    }

    public async Task<List<Server>> GetServersByUserAsync(string userId)
    {
        return await _context.ServerMembers.Where(sm => sm.UserId == userId)
                .Select(sm => sm.Server)
                .ToListAsync();
    }

    public async Task<List<ServerMember>> GetMembersAsync(Guid serverId)
    {
        return await _context.ServerMembers.Where(sm => sm.ServerId == serverId).ToListAsync();
    }
}
