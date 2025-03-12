namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

public record GetFundNotificationsResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string ResponseMessage { get; init; }
    public string ResponseCode { get; init; }

    public List<FundNotificationDataVtuNation> ResponseData { get; init; }
}
