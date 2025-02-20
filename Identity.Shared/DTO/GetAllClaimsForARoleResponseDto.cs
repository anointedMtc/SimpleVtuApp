namespace Identity.Shared.DTO;

public class GetAllClaimsForARoleResponseDto
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public Dictionary<string, List<string>> RoleClaims { get; set; } = [];

}
