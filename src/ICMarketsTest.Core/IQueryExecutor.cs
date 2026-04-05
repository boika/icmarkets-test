namespace ICMarketsTest.Core;

public interface IQueryExecutor<in TQuery, TResult>
{
    /// <summary>
    /// Executes query and returns result
    /// </summary>
    Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken = default);
}