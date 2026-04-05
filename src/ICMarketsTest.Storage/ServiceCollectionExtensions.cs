using ICMarketsTest.Core.BlockchainSnapshots;
using ICMarketsTest.Storage.BlockchainSnapshots;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ICMarketsTest.Storage;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds EF Core database context and blockchain snapshots repository implementation
    /// </summary>
    public static IServiceCollection AddEfCoreBlockchainSnapshotsRepository(this IServiceCollection services) => services
        .AddDbContext<StorageDbContext>(static (serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("Storage");
            options.UseSqlite(connectionString);
            EnsureDatabaseDirectory(connectionString);
        })
        .AddSingleton<IBlockchainSnapshotsMapper, BlockchainSnapshotsMapper>()
        .AddScoped<IBlockchainSnapshotsRepository, EfCoreBlockchainSnapshotsRepository>();

    private static void EnsureDatabaseDirectory(string? connectionString)
    {
        var dbPath = new SqliteConnectionStringBuilder(connectionString).DataSource;
        var dbDirectory = Path.GetDirectoryName(dbPath);
        if (!string.IsNullOrWhiteSpace(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }
    }

    /// <summary>
    /// Adds health check for database under EF Core
    /// </summary>
    public static IHealthChecksBuilder AddStorageHealthCheck(this IHealthChecksBuilder builder) => builder
        .AddDbContextCheck<StorageDbContext>();

    /// <summary>
    /// Ensures database under EF Core is created and migrated to the actual state
    /// </summary>
    public static void MigrateStorage(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StorageDbContext>();
        dbContext.Database.Migrate();
    }
}