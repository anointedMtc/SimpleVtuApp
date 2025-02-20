namespace Identity.Shared.DTO;

public class RemoveRoleClaimRequestDto
{
    //public Guid AppRoleId { get; set; }
    public Dictionary<string, string> RoleClaims { get; set; } = [];
}
