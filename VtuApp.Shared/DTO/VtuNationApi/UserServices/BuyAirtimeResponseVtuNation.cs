namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record BuyAirtimeResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string ResponseMessage { get; init; }
    public string ResponseCode { get; init; }
    public List<BuyAirtimeAndDataListCollection> ResponseData { get; init; }
}
