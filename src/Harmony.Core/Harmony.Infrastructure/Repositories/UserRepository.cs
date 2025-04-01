using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Domain.Entities;
using Harmony.Infrastructure.Data;

namespace Harmony.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void InsertUser(User user)
    {
        _context.Add(user);
    }
}
