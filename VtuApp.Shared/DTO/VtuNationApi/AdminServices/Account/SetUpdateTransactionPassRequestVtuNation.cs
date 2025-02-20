namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record SetUpdateTransactionPassRequestVtuNation
{
    public string OldTransactionPass { get; init; }
    public string NewTransactionPass { get; init; }
}
