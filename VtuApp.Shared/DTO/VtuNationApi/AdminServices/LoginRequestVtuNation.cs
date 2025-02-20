namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices;

public record LoginRequestVtuNation
{
    public string Phone { get; init; }
    public string Password { get; init; }
}


