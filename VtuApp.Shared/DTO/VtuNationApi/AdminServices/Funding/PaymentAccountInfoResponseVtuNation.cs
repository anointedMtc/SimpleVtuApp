namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

public record PaymentAccountInfoResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string ResponseMessage { get; init; }
    public int ResponseCode { get; init; }

    public List<AccountInfoVtuNation> ResponseData { get; init; }
}
