namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record AvailableDataNetworksResponseVtuNation
{
    public int ResponseMessage { get; init; }
    public bool IsSuccessful { get; init; }
    public int ResponseCode { get; init; }
    public List<DataResponseVtuNation> ResponseData { get; init; }
}
