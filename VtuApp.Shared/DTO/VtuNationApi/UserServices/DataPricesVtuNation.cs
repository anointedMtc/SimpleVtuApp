using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record DataPricesVtuNation
{
    public int Id { get; init; }
    public string Label { get; init; }
    public string Value { get; init; }
    public string Description { get; init; }
    public string Price { get; init; }
    public string Discount { get; init; }
    public int Status { get; init; }

    [JsonPropertyName("date_added")]
    public string DateAdded { get; init; }

    [JsonPropertyName("date_updated")]
    public string DateUpdated { get; init; }
}
