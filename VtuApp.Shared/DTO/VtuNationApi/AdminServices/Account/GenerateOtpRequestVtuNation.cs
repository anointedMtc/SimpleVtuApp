namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record GenerateOtpRequestVtuNation
{
    // example = "type":"ChangePassword"
    public string Type { get; init; }
}
