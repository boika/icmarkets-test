namespace ICMarketsTest.BlockCypher;

public class BlockCypherOptions
{
    public string BaseUrl { get; set; } = "https://api.blockcypher.com";

    public TimeSpan TotalTimeout { get; set; } = TimeSpan.FromSeconds(10);

    public TimeSpan AttemptTimeout { get; set; } = TimeSpan.FromSeconds(1);

    public int MaxRetryAttempts { get; set; } = 3;

    public TimeSpan BaseRetryDelay { get; set; } = TimeSpan.FromSeconds(1);
}