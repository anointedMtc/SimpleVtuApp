namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record ValidateOtpRequestVtuNation
{
    public string Otp { get; init; }
    public string Type { get; init; }
}
