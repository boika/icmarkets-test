using System.Data.Common;
using ICMarketsTest.BlockCypher;
using ICMarketsTest.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ICMarketsTest.WebApi.Tests.Functional;

/// <summary>
/// Mocks BlockCypher http client and uses sqlite in-memory mode
/// </summary>
/// <typeparam name="TProgram"></typeparam>
public sealed class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder
        .ConfigureTestServices(static services => services
            // Replace real-world http client with testing mock
            .AddSingleton<IBlockCypherClient, TestBlockCypherClient>()
            // Replace in-file with in-memory sqlite database
            .AddSingleton<DbConnection>(static _ =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            })
            .AddDbContext<StorageDbContext>(static (container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            }));
}