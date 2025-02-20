namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

public record SubmitPaymentNotificationRequestVtuNation
{
    public decimal Amount { get; init; }
    public string Description { get; init; }
    public string Ref {  get; init; }
}
