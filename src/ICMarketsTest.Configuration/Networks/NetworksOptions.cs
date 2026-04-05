using System.ComponentModel.DataAnnotations;

namespace ICMarketsTest.Configuration.Networks;

public class NetworksOptions
{
    [Required]
    public List<NetworkOptions> Networks { get; set; } = [];
}