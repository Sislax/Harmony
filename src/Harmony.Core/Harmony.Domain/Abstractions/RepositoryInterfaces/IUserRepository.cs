using Harmony.Domain.Entities;

namespace Harmony.Domain.Abstractions.RepositoryInterfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    void InsertUser(User user);
}
