using ICMarketsTest.Core.Networks;
using ICMarketsTest.Core.Networks.GetNetwork;

namespace ICMarketsTest.Core.Tests.Unit.Networks;

public class GetNetworkQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_Always_GetsNetworkFromRepositoryById()
    {
        // Arrange
        var id = "id_1";
        var query = new GetNetworkQuery { Id = id };
        var cancellationToken = CancellationToken.None;

        var repositoryMock = new Mock<INetworksRepository>();

        var handler = new GetNetworkQueryHandler(repositoryMock.Object);

        // Act
        await handler.HandleAsync(query, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.GetAsync(id, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Always_ReturnsNetworkFromRepository()
    {
        // Arrange
        var network = new Network
        {
            Id = "id_2",
            Name = "name_2",
            Type = "type_2"
        };
        var query = new GetNetworkQuery { Id = "id_2" };

        var repositoryMock = new Mock<INetworksRepository>();
        repositoryMock
            .Setup(r => r.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(network);

        var handler = new GetNetworkQueryHandler(repositoryMock.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        result.ShouldBeEquivalentTo(network);
    }
}