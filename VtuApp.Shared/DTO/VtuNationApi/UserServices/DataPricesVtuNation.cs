using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record DataPricesVtuNation
{
    public int Id { get; init; }
    public string Label { get; init; }
    public string Value { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public decimal Discount { get; init; }
    public int Status { get; init; }

    [JsonPropertyName("date_added")]
    public DateTime DateAdded { get; init; }

    [JsonPropertyName("date_updated")]
    public DateTime DateUpdated { get; init; }
}
