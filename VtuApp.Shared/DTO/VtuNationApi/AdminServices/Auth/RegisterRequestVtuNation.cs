using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record RegisterRequestVtuNation
{
    public string Email { get; init; }
    public string Password { get; init; }

    [JsonPropertyName("firstname")]
    public string FirstName { get; init; }

    [JsonPropertyName("lastname")]
    public string LastName { get; init; }

    [JsonPropertyName("referral_code")]
    public string ReferralCode { get; init; }

    public string Phone { get; init; }
}
