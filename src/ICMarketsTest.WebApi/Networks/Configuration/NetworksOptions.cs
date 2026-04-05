using System.ComponentModel.DataAnnotations;

namespace ICMarketsTest.WebApi.Networks.Configuration;

public class NetworksOptions
{
    [Required]
    public List<NetworkOptions> Networks { get; set; } = [];
}