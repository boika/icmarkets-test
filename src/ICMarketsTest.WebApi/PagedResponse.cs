using Swashbuckle.AspNetCore.Annotations;

namespace ICMarketsTest.WebApi;

public sealed class PagedResponse<T>
{
    [SwaggerSchema("Records data page")]
    public IReadOnlyList<T> Data { get; init; } = [];

    [SwaggerSchema("Page number")]
    public int PageNumber { get; init; }

    [SwaggerSchema("Page size")]
    public int PageSize { get; init; }

    [SwaggerSchema("Total pages count")]
    public int TotalPages { get; init; }

    [SwaggerSchema("Total records count")]
    public int TotalRecords { get; init; }

    [SwaggerSchema("Whether data has next page")]
    public bool HasNextPage => PageNumber < TotalPages;

    [SwaggerSchema("Whether data has previous page")]
    public bool HasPreviousPage => PageNumber > 1;
}