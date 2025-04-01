using Harmony.Domain.Entities;

namespace Harmony.Domain.Abstractions.RepositoryInterfaces;

public interface IUserRepository
{
    void InsertUser(User user);
}
