namespace ICMarketsTest.Core;

public sealed class PagedResult<T>
{
    /// <summary>
    /// Records data page
    /// </summary>
    public IReadOnlyList<T> Data { get; init; } = [];

    /// <summary>
    /// Page number
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Total pages count
    /// </summary>
    public int TotalPages { get; init; }

    /// <summary>
    /// Total records count
    /// </summary>
    public int TotalRecords { get; init; }

    /// <summary>
    /// Whether data has next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Whether data has previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;
}