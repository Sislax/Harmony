using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;

namespace Harmony.Infrastructure.Repositories;

public class ServerMemberRepository : GenericRepository<ServerMember>, IServerMemberRepository
{
    public ServerMemberRepository(ApplicationDbContext context) : base(context)
    {
    }
}
