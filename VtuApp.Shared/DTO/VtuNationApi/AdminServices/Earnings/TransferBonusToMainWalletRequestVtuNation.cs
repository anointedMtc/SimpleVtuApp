namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;

public record TransferBonusToMainWalletRequestVtuNation
{
    public decimal Amount { get; init; }
}
