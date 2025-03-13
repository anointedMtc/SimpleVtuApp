namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record ForgotPasswordResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
    public string ResponseCode { get; init; }
}
