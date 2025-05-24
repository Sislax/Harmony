using Harmony.Domain.Entities;

namespace Harmony.Application.Common.Interfaces.RepositoryInterfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    void InsertUser(User user);
}
