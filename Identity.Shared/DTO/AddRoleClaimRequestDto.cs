namespace Identity.Shared.DTO;

public class AddRoleClaimRequestDto
{
    //public Guid AppRoleId { get; set; }
    public Dictionary<string, string> RoleClaims { get; set; } = [];

}
