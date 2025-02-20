namespace Identity.Shared.DTO;

public class UnassignUserRoleRequestDto
{
    public string UserEmail { get; set; }
    public string RoleName { get; set; }
}
