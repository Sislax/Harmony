using System.Linq.Expressions;

namespace Harmony.Application.Common.Interfaces.RepositoryInterfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TFinalResult> GetAsync<TResult, TFinalResult>(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TResult>>? select = null,
        IQueryMaterializer<TResult, TFinalResult>? materializer = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = true,
        bool asSplitQuery = false,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);
}
