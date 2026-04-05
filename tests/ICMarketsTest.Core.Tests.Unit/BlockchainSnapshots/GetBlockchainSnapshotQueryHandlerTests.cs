using ICMarketsTest.Core.BlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshot;

namespace ICMarketsTest.Core.Tests.Unit.BlockchainSnapshots;

public class GetBlockchainSnapshotQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_Always_GetsBlockchainSnapshotFromRepositoryByIds()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        var networkId = "networkId_1";
        var cancellationToken = CancellationToken.None;
        var query = new GetBlockchainSnapshotQuery
        {
            Id = id,
            NetworkId = networkId
        };

        var repositoryMock = new Mock<IBlockchainSnapshotsRepository>();

        var handler = new GetBlockchainSnapshotQueryHandler(repositoryMock.Object);

        // Act
        await handler.HandleAsync(query, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.GetAsync(id, networkId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Always_ReturnsBlockchainSnapshotFromRepository()
    {
        // Arrange
        var snapshot = new BlockchainSnapshot
        {
            Id = Guid.CreateVersion7(),
            NetworkId = "networkId_2",
            CreatedAt = DateTimeOffset.UtcNow,
            Payload = "{\"key\":\"value\"}"
        };
        var query = new GetBlockchainSnapshotQuery
        {
            Id = Guid.CreateVersion7(),
            NetworkId = "networkId_2"
        };

        var repositoryMock = new Mock<IBlockchainSnapshotsRepository>();
        repositoryMock
            .Setup(r => r.GetAsync(query.Id, query.NetworkId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(snapshot);

        var handler = new GetBlockchainSnapshotQueryHandler(repositoryMock.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        result.ShouldBeEquivalentTo(snapshot);
    }
}