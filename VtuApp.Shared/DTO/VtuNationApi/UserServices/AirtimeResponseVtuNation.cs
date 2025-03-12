using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record AirtimeResponseVtuNation
{
    public int Id { get; init; }
    public string Label { get; init; }
    public string Discount { get; init; }
    public string Logo { get; init; }
    public string Value { get; init; }
    public string Description { get; init; }
    public int Status { get; init; }

    [JsonPropertyName("is_deleted")]
    public int IsDeleted { get; init; }

    [JsonPropertyName("date_added")]
    public string DateAdded { get; init; }

}
