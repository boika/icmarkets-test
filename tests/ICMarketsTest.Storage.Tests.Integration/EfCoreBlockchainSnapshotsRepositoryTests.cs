using ICMarketsTest.Storage.BlockchainSnapshots;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ICMarketsTest.Storage.Tests.Integration;

public class EfCoreBlockchainSnapshotsRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ServiceProvider _serviceProvider;

    public EfCoreBlockchainSnapshotsRepositoryTests()
    {
        // Use sqlite in-memory database for testing
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _serviceProvider = new ServiceCollection()
            .AddDbContext<StorageDbContext>(options => options.UseSqlite(_connection))
            .AddSingleton<IBlockchainSnapshotsMapper, BlockchainSnapshotsMapper>()
            .AddScoped<EfCoreBlockchainSnapshotsRepository>()
            .BuildServiceProvider();
    }

    [Fact]
    public async Task AddAsync_Always_SavesNewBlockchainSnapshot()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<StorageDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        var repository = scopedServices.GetRequiredService<EfCoreBlockchainSnapshotsRepository>();

        var now = DateTimeOffset.UtcNow;
        var snapshot = new Core.BlockchainSnapshots.BlockchainSnapshot
        {
            Id = Guid.CreateVersion7(now),
            CreatedAt = now.UtcDateTime,
            NetworkId = "network",
            Payload = "{}"
        };

        // Act
        await repository.AddAsync(snapshot);

        // Assert
        var added = await dbContext.BlockchainSnapshots.FindAsync(snapshot.Id);
        var expected = added.ShouldNotBeNull();
        expected.Id.ShouldBe(snapshot.Id);
        expected.NetworkId.ShouldBe(snapshot.NetworkId);
        expected.Payload.ShouldBe(snapshot.Payload);
        expected.CreatedAt.ShouldBe(now.UtcDateTime);
    }

    [Fact]
    public async Task GetAsync_WhenBlockchainSnapshotIsNotFound_ReturnsNull()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<StorageDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        var repository = scopedServices.GetRequiredService<EfCoreBlockchainSnapshotsRepository>();

        var guid1 = Guid.NewGuid();
        var network1 = "network_1";

        var guid2 = Guid.NewGuid();
        var network2 = "network_2";

        await dbContext.BlockchainSnapshots.AddRangeAsync(
            new BlockchainSnapshot { Id = guid1, NetworkId = network1, CreatedAt = DateTime.UtcNow, Payload = "{}" },
            new BlockchainSnapshot { Id = guid2, NetworkId = network2, CreatedAt = DateTime.UtcNow, Payload = "{}" });

        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetAsync(guid2, network1);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAsync_WhenBlockchainSnapshotIsFound_MapsAndReturnsBlockchainSnapshot()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<StorageDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        var repository = scopedServices.GetRequiredService<EfCoreBlockchainSnapshotsRepository>();

        var guid1 = Guid.NewGuid();
        var network1 = "network_1";

        var guid2 = Guid.NewGuid();
        var network2 = "network_2";

        await dbContext.BlockchainSnapshots.AddRangeAsync(
            new BlockchainSnapshot { Id = guid1, NetworkId = network1, CreatedAt = DateTime.UtcNow, Payload = "{}" },
            new BlockchainSnapshot { Id = guid2, NetworkId = network2, CreatedAt = DateTime.UtcNow, Payload = "{}" });

        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetAsync(guid1, network1);

        // Assert
        var snapshot = result.ShouldNotBeNull();
        snapshot.Id.ShouldBe(guid1);
        snapshot.NetworkId.ShouldBe(network1);
    }

    [Fact]
    public async Task GetPageAsync_Always_MapsAndReturnsBlockchainSnapshotsPage()
    {
        // Arrange
        using var scope = _serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<StorageDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        var repository = scopedServices.GetRequiredService<EfCoreBlockchainSnapshotsRepository>();

        var guid1 = Guid.NewGuid();
        var guid2 = Guid.NewGuid();
        var guid3 = Guid.NewGuid();
        var guid4 = Guid.NewGuid();
        var guid5 = Guid.NewGuid();

        await dbContext.BlockchainSnapshots.AddRangeAsync(
            new BlockchainSnapshot { Id = guid1, NetworkId = "network_1", CreatedAt = DateTime.UtcNow.AddHours(-5), Payload = "{}" },
            new BlockchainSnapshot { Id = guid2, NetworkId = "network_1", CreatedAt = DateTime.UtcNow.AddHours(-4), Payload = "{}" },
            new BlockchainSnapshot { Id = guid3, NetworkId = "network_2", CreatedAt = DateTime.UtcNow.AddHours(-3), Payload = "{}" },
            new BlockchainSnapshot { Id = guid4, NetworkId = "network_1", CreatedAt = DateTime.UtcNow.AddHours(-2), Payload = "{}" },
            new BlockchainSnapshot { Id = guid5, NetworkId = "network_2", CreatedAt = DateTime.UtcNow.AddHours(-1), Payload = "{}" }
        );

        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetPageAsync("network_1", 2, 2);

        // Assert
        var page = result.ShouldNotBeNull();
        page.PageNumber.ShouldBe(2);
        page.PageSize.ShouldBe(2);
        page.TotalPages.ShouldBe(2);
        page.TotalRecords.ShouldBe(3);
        page.HasNextPage.ShouldBeFalse();
        page.HasPreviousPage.ShouldBeTrue();
        page.Data.ShouldHaveSingleItem().Id.ShouldBe(guid1);
    }

    public void Dispose() => _connection.Close();
}