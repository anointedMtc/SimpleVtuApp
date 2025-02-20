using System.Text.Json.Serialization;

namespace ExternalServices.Application.TypiCodeService.Services;

public class ExternalApiKeyHolder
{
    [JsonPropertyName("email")]
    public string? AccessToken { get; set; } = string.Empty;
}
