namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Complaint;

public record AddComplaintResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
    public string ResponseCode { get; init; }
}
