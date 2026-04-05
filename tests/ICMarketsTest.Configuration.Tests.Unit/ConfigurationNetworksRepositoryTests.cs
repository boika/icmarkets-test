using ICMarketsTest.Configuration.Networks;
using ICMarketsTest.Core.Networks;
using Microsoft.Extensions.Options;

namespace ICMarketsTest.Configuration.Tests.Unit;

public class ConfigurationNetworksRepositoryTests
{
    [Fact]
    public async Task GetAsync_WhenNetworkIsNotFound_ReturnsNull()
    {
        // Arrange
        var id = "id_135";
        var optionsSnapshot = new Mock<IOptionsSnapshot<NetworksOptions>>();
        optionsSnapshot.Setup(x => x.Value).Returns(new NetworksOptions
        {
            Networks = []
        });

        var repository = new ConfigurationNetworksRepository(optionsSnapshot.Object, Mock.Of<INetworksMapper>());

        // Act
        var result = await repository.GetAsync(id);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAsync_WhenNetworkIsFound_MapsAndReturnsNetwork()
    {
        // Arrange
        var id = "id_135";
        var name = "id_135";
        var type = "type_135";
        var optionsSnapshot = new Mock<IOptionsSnapshot<NetworksOptions>>();
        optionsSnapshot.Setup(x => x.Value).Returns(new NetworksOptions
        {
            Networks =
            [
                new NetworkOptions { Id = "id_1", Name = "name_1", Type = "type_1" },
                new NetworkOptions { Id = id, Name = name, Type = type },
                new NetworkOptions { Id = "id_3", Name = "name_3", Type = "type_3" }
            ]
        });

        var network = new Network { Id = id, Name = name, Type = type };
        var mapper = new Mock<INetworksMapper>();
        mapper
            .Setup(m => m.Map(It.IsAny<NetworkOptions>()))
            .Returns(network);

        var repository = new ConfigurationNetworksRepository(optionsSnapshot.Object, mapper.Object);

        // Act
        var result = await repository.GetAsync(id);

        // Assert
        result.ShouldBeEquivalentTo(network);
    }

    [Fact]
    public async Task GetPageAsync_Always_MapsAndReturnsNetworksPage()
    {
        // Arrange
        var optionsSnapshot = new Mock<IOptionsSnapshot<NetworksOptions>>();
        optionsSnapshot.Setup(x => x.Value).Returns(new NetworksOptions
        {
            Networks =
            [
                new NetworkOptions { Id = "id_1", Name = "name_1", Type = "type_1" },
                new NetworkOptions { Id = "id_2", Name = "name_2", Type = "type_2" },
                new NetworkOptions { Id = "id_3", Name = "name_3", Type = "type_3" },
                new NetworkOptions { Id = "id_4", Name = "name_4", Type = "type_4" },
                new NetworkOptions { Id = "id_5", Name = "name_5", Type = "type_5" }
            ]
        });

        var repository = new ConfigurationNetworksRepository(optionsSnapshot.Object, new NetworksMapper());

        // Act
        var result = await repository.GetPageAsync(3, 2);

        // Assert
        var page = result.ShouldNotBeNull();
        page.PageNumber.ShouldBe(3);
        page.PageSize.ShouldBe(2);
        page.TotalPages.ShouldBe(3);
        page.TotalRecords.ShouldBe(5);
        page.HasNextPage.ShouldBeFalse();
        page.HasPreviousPage.ShouldBeTrue();
        page.Data.ShouldHaveSingleItem().Id.ShouldBe("id_5");
    }
}