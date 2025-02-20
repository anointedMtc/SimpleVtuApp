namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record AvailableDataPricesVtuNation
{
    public string ResponseMessage { get; init; }
    public int ResponseCode { get; init; }
    public List<DataPricesVtuNation> ResponseData { get; init; }
}
