using System.ComponentModel.DataAnnotations;

namespace ICMarketsTest.WebApi.Networks.Configuration;

public class NetworkOptions
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(32)]
    public string Id { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(32)]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(32)]
    public string Type { get; set; } = string.Empty;
}