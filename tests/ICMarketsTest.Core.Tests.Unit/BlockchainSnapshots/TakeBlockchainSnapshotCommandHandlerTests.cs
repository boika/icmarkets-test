using ICMarketsTest.Core.BlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.TakeBlockchainSnapshot;
using Microsoft.Extensions.Time.Testing;

namespace ICMarketsTest.Core.Tests.Unit.BlockchainSnapshots;

public class TakeBlockchainSnapshotCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Always_GetsBlockchainSnapshotFromProviderByNameAndType()
    {
        // Arrange
        var networkId = "id_7";
        var networkName = "name_7";
        var networkType = "type_7";
        var cancellationToken = CancellationToken.None;
        var command = new TakeBlockchainSnapshotCommand
        {
            NetworkId = networkId,
            NetworkName = networkName,
            NetworkType = networkType
        };

        var utcNow = DateTimeOffset.UtcNow;
        var clock = new FakeTimeProvider();
        clock.SetUtcNow(utcNow);

        var providerMock = new Mock<IBlockchainSnapshotsProvider>();

        var handler = new TakeBlockchainSnapshotCommandHandler(
            providerMock.Object,
            Mock.Of<IBlockchainSnapshotsRepository>(),
            clock);

        // Act
        await handler.HandleAsync(command, cancellationToken);

        // Assert
        providerMock.Verify(
            p => p.GetBlockchainSnapshotAsync(networkName, networkType, cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Always_AddsBlockchainSnapshotToRepository()
    {
        // Arrange
        var networkId = "id_8";
        var networkName = "name_8";
        var networkType = "type_8";
        var payload = "{\"key\":\"value\"}";
        var cancellationToken = CancellationToken.None;
        var command = new TakeBlockchainSnapshotCommand
        {
            NetworkId = networkId,
            NetworkName = networkName,
            NetworkType = networkType
        };

        var utcNow = DateTimeOffset.UtcNow;
        var clock = new FakeTimeProvider();
        clock.SetUtcNow(utcNow);

        var providerMock = new Mock<IBlockchainSnapshotsProvider>();
        providerMock
            .Setup(p => p.GetBlockchainSnapshotAsync(networkName, networkType, cancellationToken))
            .ReturnsAsync(payload);

        var repositoryMock = new Mock<IBlockchainSnapshotsRepository>();

        var handler = new TakeBlockchainSnapshotCommandHandler(providerMock.Object, repositoryMock.Object, clock);

        // Act
        await handler.HandleAsync(command, cancellationToken);

        // Assert
        repositoryMock.Verify(
            r => r.AddAsync(It.Is<BlockchainSnapshot>(s =>
                    s.NetworkId == networkId &&
                    s.CreatedAt == utcNow &&
                    s.Payload == payload),
                cancellationToken),
            Times.Once);
    }
}