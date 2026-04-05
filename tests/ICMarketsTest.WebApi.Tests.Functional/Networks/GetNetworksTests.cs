using System.Net;
using System.Text.Json;
using ICMarketsTest.WebApi.Networks.GetNetwork;

namespace ICMarketsTest.WebApi.Tests.Functional.Networks;

public class GetNetworksTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetNetworksTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetNetworks_WithoutQueryParameters_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/networks");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var networks = JsonSerializer
            .Deserialize<PagedResponse<GetNetworkResponse>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .ShouldNotBeNull();

        networks.Data.Count.ShouldBe(5);
        networks.PageNumber.ShouldBe(1);
        networks.PageSize.ShouldBe(10);
        networks.TotalPages.ShouldBe(1);
        networks.TotalRecords.ShouldBe(5);
        networks.HasNextPage.ShouldBeFalse();
        networks.HasPreviousPage.ShouldBeFalse();
    }

    [Fact]
    public async Task GetNetworks_WithValidQueryParameters_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/networks?pageNumber=2&pageSize=4");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var networks = JsonSerializer
            .Deserialize<PagedResponse<GetNetworkResponse>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .ShouldNotBeNull();

        networks.Data.Count.ShouldBe(1);
        networks.PageNumber.ShouldBe(2);
        networks.PageSize.ShouldBe(4);
        networks.TotalPages.ShouldBe(2);
        networks.TotalRecords.ShouldBe(5);
        networks.HasNextPage.ShouldBeFalse();
        networks.HasPreviousPage.ShouldBeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetNetworks_WithInvalidPageNumber_ReturnsBadRequest(int pageNumber)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/v1/networks?pageNumber={pageNumber}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(101)]
    [InlineData(3450)]
    public async Task GetNetworks_WithInvalidPageSize_ReturnsBadRequest(int pageSize)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/v1/networks?pageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}