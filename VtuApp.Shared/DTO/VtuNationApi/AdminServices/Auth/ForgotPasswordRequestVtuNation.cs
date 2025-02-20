using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record ForgotPasswordRequestVtuNation
{
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; init; }
}
