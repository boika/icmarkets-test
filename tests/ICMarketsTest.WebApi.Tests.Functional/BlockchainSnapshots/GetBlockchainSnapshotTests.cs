using System.Net;
using System.Text.Json;
using ICMarketsTest.WebApi.BlockchainSnapshots.GetBlockchainSnapshot;

namespace ICMarketsTest.WebApi.Tests.Functional.BlockchainSnapshots;

public class GetBlockchainSnapshotTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetBlockchainSnapshotTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetBlockchainSnapshot_WhenGetBlockchainSnapshotIsFound_ReturnsOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        await client.PostAsync("/api/v1/networks/dash-main/snapshots", null);

        var response = await client.GetAsync("/api/v1/networks/dash-main/snapshots");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var snapshot = JsonSerializer
            .Deserialize<PagedResponse<GetBlockchainSnapshotResponse>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .ShouldNotBeNull()
            .Data.First();

        // Act
        response = await client.GetAsync($"/api/v1/networks/{snapshot.NetworkId}/snapshots/{snapshot.Id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        content = await response.Content.ReadAsStringAsync();
        var actualSnapshot = JsonSerializer
            .Deserialize<GetBlockchainSnapshotResponse>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .ShouldNotBeNull();

        actualSnapshot.Id.ShouldBe(snapshot.Id);
        actualSnapshot.NetworkId.ShouldBe(snapshot.NetworkId);
        actualSnapshot.CreatedAt.ShouldBe(snapshot.CreatedAt);
    }

    [Fact]
    public async Task GetBlockchainSnapshot_WhenBlockchainSnapshotIsNotFound_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/networks/unknown/snapshots/b370c15b20c84447b3d0979c089ddbd9");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetBlockchainSnapshot_WhenNetworkIdIsTooLong_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/networks/abcdefghijklmnopabcdefghijklmnopabcdefghijklmnop/snapshots/2b958b989e0140dcb0b27be7330dbe5b");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetBlockchainSnapshot_WhenSnapshotIdIsTooLong_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/networks/btc-test3/snapshots/abcdefghijklmnopabcdefghijklmnopabcdefghijklmnop");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}