namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Auth;

public record LogOutResponseVtuNation
{
    public bool IsSuccessful { get; init; }
    public string Message { get; init; }
}
