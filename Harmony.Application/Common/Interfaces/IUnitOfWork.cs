namespace Harmony.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
