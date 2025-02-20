namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record AvailableAirtimeNetworksResponseVtuNation
{
    public int ResponseMessage { get; init; }
    public bool IsSuccessful { get; init; }
    public int ResponseCode { get; init; }
    public List<AirtimeResponseVtuNation> ResponseData { get; init; }

}
