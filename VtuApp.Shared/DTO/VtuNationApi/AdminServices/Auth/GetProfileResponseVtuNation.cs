using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record GetProfileResponseVtuNation
{
    public int Id { get; init; }


    [JsonPropertyName("username")]
    public string UserName { get; init; }


    [JsonPropertyName("firstname")]
    public string FirstName { get; init; }


    [JsonPropertyName("lastname")]
    public string LastName { get; init; }


    [JsonPropertyName("phone")]
    public string PhoneNumber { get; init; }


    [JsonPropertyName("referral_code")]
    public string ReferralCode { get; init; }


    [JsonPropertyName("balance")]
    public string Balance { get; init; }


    [JsonPropertyName("bonus_balance")]
    public string BonusBalance { get; init; }


    public string Email { get; init; }

    public string Code { get; init; }

    public string Status { get; init; }

    [JsonPropertyName("account_type")]
    public string AccountType { get; init; }
}
