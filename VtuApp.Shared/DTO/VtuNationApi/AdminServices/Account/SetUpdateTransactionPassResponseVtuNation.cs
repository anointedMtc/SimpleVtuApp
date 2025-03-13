namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Account;

public record SetUpdateTransactionPassResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
    public string ResponseCode { get; init; }
}
