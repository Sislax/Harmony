using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;

namespace Harmony.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
