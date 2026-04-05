using System.Net;
using System.Text.Json;
using ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshot;

namespace ICMarketsTest.WebApi.Tests.Functional.BlockchainSnapshots;

public class GetBlockchainSnapshotsTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetBlockchainSnapshotsTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetBlockchainSnapshots_WithoutQueryParameters_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();

        await client.PostAsync("/api/v1/networks/btc-main/snapshots", null);
        await Task.Delay(50);
        await client.PostAsync("/api/v1/networks/btc-main/snapshots", null);

        // Act
        var response = await client.GetAsync("/api/v1/networks/btc-main/snapshots");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var networks = JsonSerializer
            .Deserialize<PagedResponse<GetBlockchainSnapshotResponse>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .ShouldNotBeNull();

        networks.Data.Count.ShouldBe(2);
        networks.PageNumber.ShouldBe(1);
        networks.PageSize.ShouldBe(10);
        networks.TotalPages.ShouldBe(1);
        networks.TotalRecords.ShouldBe(2);
        networks.HasNextPage.ShouldBeFalse();
        networks.HasPreviousPage.ShouldBeFalse();
    }

    [Fact]
    public async Task GetBlockchainSnapshots_WithValidQueryParameters_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        await client.PostAsync("/api/v1/networks/ltc-main/snapshots", null);
        await Task.Delay(50);
        await client.PostAsync("/api/v1/networks/ltc-main/snapshots", null);

        // Act
        var response = await client.GetAsync("/api/v1/networks/ltc-main/snapshots?pageNumber=1&pageSize=1");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var networks = JsonSerializer
            .Deserialize<PagedResponse<GetBlockchainSnapshotResponse>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .ShouldNotBeNull();

        networks.Data.Count.ShouldBe(1);
        networks.PageNumber.ShouldBe(1);
        networks.PageSize.ShouldBe(1);
        networks.TotalPages.ShouldBe(2);
        networks.TotalRecords.ShouldBe(2);
        networks.HasNextPage.ShouldBeTrue();
        networks.HasPreviousPage.ShouldBeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public async Task GetBlockchainSnapshots_WithInvalidPageNumber_ReturnsBadRequest(int pageNumber)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/v1/networks/btc-main/snapshots?pageNumber={pageNumber}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(101)]
    [InlineData(3450)]
    public async Task GetBlockchainSnapshots_WithInvalidPageSize_ReturnsBadRequest(int pageSize)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/v1/networks/eth-main/snapshots?pageSize={pageSize}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}