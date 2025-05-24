using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;

namespace Harmony.Infrastructure.Repositories;

public class ServerRepository : GenericRepository<Server>, IServerRepository
{

    public ServerRepository(ApplicationDbContext context) : base(context)
    {
    }

    //public async Task<List<Server>> GetServersByUserAsync(string userId)
    //{
    //    return await _context.ServerMembers.Where(sm => sm.UserId == userId)
    //            .Select(sm => sm.Server)
    //            .ToListAsync();
    //}

    //public async Task<List<ServerMember>> GetMembersAsync(Guid serverId)
    //{
    //    return await _context.ServerMembers.Where(sm => sm.ServerId == serverId).ToListAsync();
    //}
}
