using System.Net;
using System.Text.Json;
using ICMarketsTest.WebApi.Networks.GetNetwork;

namespace ICMarketsTest.WebApi.Tests.Functional.Networks;

public class GetNetworkTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;

    public GetNetworkTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("eth-main")]
    [InlineData("dash-main")]
    [InlineData("btc-main")]
    [InlineData("btc-test3")]
    [InlineData("ltc-main")]
    public async Task GetNetwork_WhenNetworkIsFound_ReturnsOk(string networkId)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/v1/networks/{networkId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var network = JsonSerializer
            .Deserialize<GetNetworkResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            .ShouldNotBeNull();

        network.Id.ShouldBe(networkId);
    }

    [Fact]
    public async Task GetNetwork_WhenNetworkIsNotFound_ReturnsNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/networks/unknown");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetNetwork_WhenNetworkIdIsTooLong_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/networks/abcdefghijklmnopabcdefghijklmnopabcdefghijklmnop");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}