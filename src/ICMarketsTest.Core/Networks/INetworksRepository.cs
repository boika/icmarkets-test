namespace ICMarketsTest.Core.Networks;

public interface INetworksRepository
{
    /// <summary>
    /// Gets single network by identifier
    /// </summary>
    Task<Network?> GetAsync(
        string id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paged list of networks in ascending order
    /// </summary>
    Task<PagedResult<Network>> GetPageAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}