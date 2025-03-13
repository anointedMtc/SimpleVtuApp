namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

public record SubmitPaymentNotificationResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string ResponseMessage { get; init; }
    public string ResponseCode { get; init; }
}
