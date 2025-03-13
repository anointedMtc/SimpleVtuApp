namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record AvailableAirtimeNetworksResponseVtuNation
{
    public string ResponseMessage { get; init; }
    public bool IsSuccessful { get; init; }
    public string ResponseCode { get; init; }
    public List<AirtimeResponseVtuNation> ResponseData { get; init; }

}
