namespace ICMarketsTest.BlockCypher.Tests.Unit;

public class BlockCypherBlockchainSnapshotsProviderTests
{
    [Fact]
    public async Task GetBlockchainSnapshotAsync_Always_GetsSnapshotByNameAndType()
    {
        // Arrange
        var name = "name_1";
        var type = "type_1";
        var cancellationToken = CancellationToken.None;

        var clientMock = new Mock<IBlockCypherClient>();

        var provider = new BlockCypherBlockchainSnapshotsProvider(clientMock.Object);

        // Act
        await provider.GetBlockchainSnapshotAsync(name, type, cancellationToken);

        // Assert
        clientMock.Verify(c => c.GetSnapshotAsync(name, type, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetBlockchainSnapshotAsync_Always_ReturnsSnapshotFromClient()
    {
        // Arrange
        var payload = "{\"key\":\"value\"}";

        var clientMock = new Mock<IBlockCypherClient>();
        clientMock
            .Setup(c => c.GetSnapshotAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(payload);

        var provider = new BlockCypherBlockchainSnapshotsProvider(clientMock.Object);

        // Act
        var result = await provider.GetBlockchainSnapshotAsync("name", "type");

        // Assert
        result.ShouldBe(payload);
    }
}