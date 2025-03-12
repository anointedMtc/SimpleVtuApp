namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record AvailableDataNetworksResponseVtuNation
{
    public string ResponseMessage { get; init; }
    public bool IsSuccessful { get; init; }
    public string ResponseCode { get; init; }
    public List<DataResponseVtuNation> ResponseData { get; init; }
}
