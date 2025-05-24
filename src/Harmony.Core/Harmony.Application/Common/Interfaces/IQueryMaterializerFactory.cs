using System.Linq.Expressions;

namespace Harmony.Application.Common.Interfaces;

public interface IQueryMaterializerFactory
{
    IQueryMaterializer<TResult, TResult> FirstAsync<TResult>();
    IQueryMaterializer<TResult, TResult?> FirstOrDefaultAsync<TResult>();
    IQueryMaterializer<TResult, TResult> SingleAsync<TResult>();
    IQueryMaterializer<TResult, TResult?> SingleOrDefaultAsync<TResult>();
    IQueryMaterializer<TResult, List<TResult>> ToListAsync<TResult>();
    IQueryMaterializer<TResult, TResult[]> ToArrayAsync<TResult>();
    IQueryMaterializer<TResult, Dictionary<TKey, TResult>> ToDictionaryAsync<TKey, TResult>(Func<TResult, TKey> keySelector) where TKey : notnull;
    IQueryMaterializer<TResult, int> CountAsync<TResult>();
    IQueryMaterializer<TResult, long> LongCountAsync<TResult>();
    IQueryMaterializer<TResult, bool> AnyAsync<TResult>();
    IQueryMaterializer<TResult, bool> AllAsync<TResult>(Expression<Func<TResult, bool>> predicate);
}
