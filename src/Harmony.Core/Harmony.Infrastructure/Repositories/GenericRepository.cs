using System.Linq.Expressions;
using Harmony.Application.Common.Interfaces;
using Harmony.Application.Common.Interfaces.RepositoryInterfaces;
using Harmony.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    internal readonly ApplicationDbContext _context;
    internal readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TFinalResult> GetAsync<TResult, TFinalResult>(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TResult>>? select = null,
        IQueryMaterializer<TResult, TFinalResult>? materializer = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracked = true,
        bool asSplitQuery = false,
        int? skip = null,
        int? take = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (asSplitQuery)
        {
            query = query.AsSplitQuery();
        }
            
        if (include != null)
        {
            query = include(query);
        }
            
        if (filter != null)
        {
            query = query.Where(filter);
        }
            
        if (orderBy != null)
        {
            query = orderBy(query);
        }
            
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
            
        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        IQueryable<TResult> projected;

        if (select != null)
        {
            projected = query.Select(select);
        }
        else if (typeof(TEntity) == typeof(TResult))
        {
            projected = (IQueryable<TResult>)query;
        }
        else
        {
            throw new InvalidOperationException("Select must be provided if TResult is not TEntity.");
        }

        if(materializer == null)
        {
            throw new ArgumentNullException(nameof(materializer), "Materializer must be provided.");
        }

        return await materializer.MaterializeAsync(projected, cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}
