using System.Net;

namespace ICMarketsTest.WebApi.Tests.Functional.BlockchainSnapshots;

public class TakeBlockchainSnapshotTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public TakeBlockchainSnapshotTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("eth-main")]
    [InlineData("btc-test3")]
    public async Task TakeBlockchainSnapshot_WhenNetworkIsFound_ReturnsOk(string networkId)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync($"/api/v1/networks/{networkId}/snapshots", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBeEmpty();
    }

    [Fact]
    public async Task TakeBlockchainSnapshot_WhenNetworkIsNotFound_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/v1/networks/unknown/snapshots", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TakeBlockchainSnapshot_WhenNetworkIdIsTooLong_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("/api/v1/networks/abcdefghijklmnopabcdefghijklmnopabcdefghijklmnop/snapshots", null);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}