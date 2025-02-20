namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;

public record TransferBonusToMainWalletResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string ResponseMessage { get; init; }
    public int ResponseCode { get; init; }
}
