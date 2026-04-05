using System.ComponentModel.DataAnnotations;

namespace ICMarketsTest.BlockCypher;

public class BlockCypherOptions
{
    [Required(AllowEmptyStrings = false)]
    public string BaseUrl { get; set; } = "https://api.blockcypher.com";

    public TimeSpan TotalTimeout { get; set; } = TimeSpan.FromSeconds(10);

    public TimeSpan AttemptTimeout { get; set; } = TimeSpan.FromSeconds(1);

    [Range(0, 10)]
    public int MaxRetryAttempts { get; set; } = 3;

    public TimeSpan BaseRetryDelay { get; set; } = TimeSpan.FromSeconds(1);
}