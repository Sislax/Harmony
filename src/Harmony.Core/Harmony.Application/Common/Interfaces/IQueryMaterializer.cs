namespace Harmony.Application.Common.Interfaces;

public interface IQueryMaterializer<TResult, TFinalResult>
{
    Task<TFinalResult> MaterializeAsync(IQueryable<TResult> query, CancellationToken cancellationToken = default);
}
