using ICMarketsTest.Core.Networks;
using ICMarketsTest.Core.Networks.GetNetworks;

namespace ICMarketsTest.Core.Tests.Unit.Networks;

public class GetNetworksQueryHandlerTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(15, 15)]
    [InlineData(0, 1)]
    [InlineData(-57, 1)]
    public async Task HandleAsync_Always_NormalizesPageNumber(int actualPageNumber, int expectedPageNumber)
    {
        // Arrange
        var query = new GetNetworksQuery
        {
            PageNumber = actualPageNumber,
            PageSize = 10
        };

        var repositoryMock = new Mock<INetworksRepository>();

        var handler = new GetNetworksQueryHandler(repositoryMock.Object);

        // Act
        await handler.HandleAsync(query);

        // Assert
        repositoryMock.Verify(
            r => r.GetPageAsync(expectedPageNumber, It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(0, 1)]
    [InlineData(-55, 1)]
    [InlineData(73, 73)]
    [InlineData(100, 100)]
    [InlineData(101, 100)]
    [InlineData(572, 100)]
    public async Task HandleAsync_Always_NormalizesPageSize(int actualPageSize, int expectedPageSize)
    {
        // Arrange
        var query = new GetNetworksQuery
        {
            PageNumber = 1,
            PageSize = actualPageSize
        };

        var repositoryMock = new Mock<INetworksRepository>();

        var handler = new GetNetworksQueryHandler(repositoryMock.Object);

        // Act
        await handler.HandleAsync(query);

        // Assert
        repositoryMock.Verify(
            r => r.GetPageAsync(It.IsAny<int>(), expectedPageSize, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Always_ReturnsPageFromRepository()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 3;
        var page = new PagedResult<Network>
        {
            Data =
            [
                new Network { Id = "id_1", Name = "name_1", Type = "type_1" },
                new Network { Id = "id_2", Name = "name_2", Type = "type_2" },
                new Network { Id = "id_3", Name = "name_3", Type = "type_3" }
            ],
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = 5,
            TotalRecords = 14
        };
        var query = new GetNetworksQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var repositoryMock = new Mock<INetworksRepository>();
        repositoryMock
            .Setup(r => r.GetPageAsync(pageNumber, pageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        var handler = new GetNetworksQueryHandler(repositoryMock.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        result.ShouldBeEquivalentTo(page);
    }
}