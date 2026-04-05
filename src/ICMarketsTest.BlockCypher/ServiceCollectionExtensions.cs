using System.Net.Sockets;
using System.Threading.RateLimiting;
using ICMarketsTest.Core.BlockchainSnapshots;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using Polly.RateLimiting;
using Polly.Timeout;
using Refit;

namespace ICMarketsTest.BlockCypher;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds BlockCypher API client along with resilience strategy (timeouts, retries, rate limiting)
    /// and blockchain snapshots provider implementation
    /// </summary>
    public static IServiceCollection AddBlockCypherBlockchainSnapshotsProvider(this IServiceCollection services)
    {
        services
            .AddOptions<BlockCypherOptions>()
            .BindConfiguration(nameof(BlockCypherOptions));

        services
            .AddRefitClient<IBlockCypherClient>()
            .ConfigureHttpClient(static (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BlockCypherOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddResilienceHandler(nameof(IBlockCypherClient), static (builder, context) =>
            {
                var options = context.ServiceProvider.GetRequiredService<IOptions<BlockCypherOptions>>().Value;

                builder
                    // 1. Set global timeout for all the attempts (10 seconds by default)
                    .AddTimeout(options.TotalTimeout)
                    // 2. Set retries with exponential backoff + jitter (3 more attempts by default)
                    .AddRetry(new HttpRetryStrategyOptions
                    {
                        MaxRetryAttempts = options.MaxRetryAttempts,
                        BackoffType = DelayBackoffType.Exponential,
                        Delay = options.BaseRetryDelay,
                        UseJitter = true,
                        ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                            .Handle<SocketException>()
                            .Handle<HttpRequestException>()
                            .Handle<TimeoutRejectedException>()
                            .Handle<RateLimiterRejectedException>()
                            .HandleResult(response => !response.IsSuccessStatusCode)
                    })
                    // 3. Set rate limiters according to https://www.blockcypher.com/dev/bitcoin/#rate-limits-and-tokens
                    .AddRateLimiter(new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 3, // 3 requests per second, window moves every half a second
                        Window = TimeSpan.FromSeconds(1),
                        SegmentsPerWindow = 2
                    }))
                    .AddRateLimiter(new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 100, // 100 requests per hour, window moves every minute
                        Window = TimeSpan.FromHours(1),
                        SegmentsPerWindow = 60
                    }))
                    // 4. Set local timeout for each individual attempt (1 second by default)
                    .AddTimeout(options.AttemptTimeout);
            });

        return services
            .AddScoped<IBlockchainSnapshotsProvider, BlockCypherBlockchainSnapshotsProvider>();
    }
}