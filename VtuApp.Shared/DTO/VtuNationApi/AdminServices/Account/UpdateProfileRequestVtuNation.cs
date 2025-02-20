using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record UpdateProfileRequestVtuNation
{
    [JsonPropertyName("firstname")]
    public string FirstName { get; init; }


    [JsonPropertyName("lastname")]
    public string LastName { get; init; }
}
