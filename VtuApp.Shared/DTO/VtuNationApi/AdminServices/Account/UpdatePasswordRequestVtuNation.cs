using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record UpdatePasswordRequestVtuNation
{
    // You have to generate Otp before you can change your password
    //public string OldPassword { get; init; }

    [JsonPropertyName("temp_token")]
    public string OtpToken { get; init; }
    public string NewPassword { get; init; }
}
