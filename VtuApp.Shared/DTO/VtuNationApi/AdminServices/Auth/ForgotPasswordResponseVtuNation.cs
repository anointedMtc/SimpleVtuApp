namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record ForgotPasswordResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
    public int ResponseCode { get; init; }
}
