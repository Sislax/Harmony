using System.Linq.Expressions;
using Harmony.Application.Common.Interfaces;
using Harmony.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories;

public class GenericRepository<TEntity> : Application.Common.Interfaces.RepositoryInterfaces.IGenericRepository<TEntity> where TEntity : class
{
    internal readonly ApplicationDbContext _context;
    internal readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    // Considerare bene se fare questo metodo generics per restituire un result di un certo tipo, ad esempio DTO
    //public virtual async Task<List<TEntity>> GetManyAsync<TResult>(
    //    Expression<Func<TEntity, bool>>? filter = null,
    //    Expression<Func<TEntity, TResult>>? select = null,
    //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    //    string includeProperties = "",
    //    bool tracked = true)
    //{
    //    IQueryable<TEntity> query = _dbSet;
    //
    //    if(filter != null)
    //    {
    //        query = query.Where(filter);
    //    }
    //
    //    foreach(string includeProperiey in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
    //    {
    //        query = query.Include(includeProperiey);
    //    }
    //
    //    if(orderBy != null)
    //    {
    //        return await orderBy(query).ToListAsync();
    //    }
    //    else
    //    {
    //        return await query.ToListAsync();
    //    }
    //}

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

    public virtual void CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}
