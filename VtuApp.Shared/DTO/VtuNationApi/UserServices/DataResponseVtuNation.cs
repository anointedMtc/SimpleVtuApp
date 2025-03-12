using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record DataResponseVtuNation
{
    public int Id { get; init; }
    public string Label { get; init; }
    public string Logo { get; init; }
    public string Value { get; init; }
    public string Description { get; init; }
    public int Status { get; init; }

    [JsonPropertyName("date_added")]
    public string DateAdded { get; init; }
}
