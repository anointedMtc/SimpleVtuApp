using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

public record FundNotificationDataVtuNation
{
    public int Id { get; init; }

    [JsonPropertyName("username")]
    public string UserName { get; init; }

    public string Amount { get; init; }
    public string Description { get; init; }
    public string Ref {  get; init; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; init; }
}
