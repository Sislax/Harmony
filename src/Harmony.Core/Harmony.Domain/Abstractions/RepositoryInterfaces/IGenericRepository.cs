using System.Linq.Expressions;

namespace Harmony.Domain.Abstractions.RepositoryInterfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetManyAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        bool tracked = true);

    Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        string includeProperties = "",
        bool tracked = true);

    void CreateAsync(TEntity entity);

    void UpdateAsync(TEntity entity);

    void DeleteAsync(TEntity entity);
}
