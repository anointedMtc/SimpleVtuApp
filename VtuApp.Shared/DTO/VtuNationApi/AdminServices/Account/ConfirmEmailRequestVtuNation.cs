namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record ConfirmEmailRequestVtuNation
{
    public string VerificationId { get; init; }
}
