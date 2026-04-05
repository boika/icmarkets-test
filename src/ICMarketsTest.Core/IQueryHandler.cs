namespace ICMarketsTest.Core;

public interface IQueryHandler<in TQuery, TResult>
{
    /// <summary>
    /// Handles query and returns result
    /// </summary>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}