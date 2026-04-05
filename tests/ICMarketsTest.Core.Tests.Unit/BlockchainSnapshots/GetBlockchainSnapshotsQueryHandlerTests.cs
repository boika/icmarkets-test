using ICMarketsTest.Core.BlockchainSnapshots;
using ICMarketsTest.Core.BlockchainSnapshots.GetBlockchainSnapshots;

namespace ICMarketsTest.Core.Tests.Unit.BlockchainSnapshots;

public class GetBlockchainSnapshotsQueryHandlerTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(31, 31)]
    [InlineData(0, 1)]
    [InlineData(-170, 1)]
    public async Task HandleAsync_Always_NormalizesPageNumber(int actualPageNumber, int expectedPageNumber)
    {
        // Arrange
        var networkId = "id_3";
        var query = new GetBlockchainSnapshotsQuery
        {
            NetworkId = networkId,
            PageNumber = actualPageNumber,
            PageSize = 10
        };

        var repositoryMock = new Mock<IBlockchainSnapshotsRepository>();

        var handler = new GetBlockchainSnapshotsQueryHandler(repositoryMock.Object);

        // Act
        await handler.HandleAsync(query);

        // Assert
        repositoryMock.Verify(
            r => r.GetPageAsync(networkId, expectedPageNumber, It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(0, 1)]
    [InlineData(-12, 1)]
    [InlineData(50, 50)]
    [InlineData(100, 100)]
    [InlineData(101, 100)]
    [InlineData(371, 100)]
    public async Task HandleAsync_Always_NormalizesPageSize(int actualPageSize, int expectedPageSize)
    {
        // Arrange
        var networkId = "id_4";
        var query = new GetBlockchainSnapshotsQuery
        {
            NetworkId = networkId,
            PageNumber = 1,
            PageSize = actualPageSize
        };

        var repositoryMock = new Mock<IBlockchainSnapshotsRepository>();

        var handler = new GetBlockchainSnapshotsQueryHandler(repositoryMock.Object);

        // Act
        await handler.HandleAsync(query);

        // Assert
        repositoryMock.Verify(
            r => r.GetPageAsync(networkId, It.IsAny<int>(), expectedPageSize, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Always_ReturnsPageFromRepository()
    {
        // Arrange
        var networkId = "id_5";
        var pageNumber = 1;
        var pageSize = 2;
        var page = new PagedResult<BlockchainSnapshot>
        {
            Data =
            [
                new BlockchainSnapshot
                {
                    Id = Guid.CreateVersion7(),
                    NetworkId = networkId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Payload = "{\"key\":\"value1\"}"
                },
                new BlockchainSnapshot
                {
                    Id = Guid.CreateVersion7(),
                    NetworkId = networkId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Payload = "{\"key\":\"value2\"}"
                }
            ],
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = 3,
            TotalRecords = 5
        };
        var query = new GetBlockchainSnapshotsQuery
        {
            NetworkId = networkId,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var repositoryMock = new Mock<IBlockchainSnapshotsRepository>();
        repositoryMock
            .Setup(r => r.GetPageAsync(networkId, pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        var handler = new GetBlockchainSnapshotsQueryHandler(repositoryMock.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        result.ShouldBeEquivalentTo(page);
    }
}