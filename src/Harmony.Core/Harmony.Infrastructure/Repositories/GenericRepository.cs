using System.Linq.Expressions;
using Harmony.Domain.Abstractions.RepositoryInterfaces;
using Harmony.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Harmony.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetManyAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        bool tracked = true)
    {
        IQueryable<TEntity> query = _dbSet;

        if(filter != null)
        {
            query = query.Where(filter);
        }

        foreach(string includeProperiey in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperiey);
        }

        if(orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public virtual async Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        string includeProperties = "",
        bool tracked = true)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (string includeProperiey in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperiey);
        }

        return await query.SingleOrDefaultAsync();
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
