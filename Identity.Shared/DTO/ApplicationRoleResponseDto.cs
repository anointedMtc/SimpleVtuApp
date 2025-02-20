namespace Identity.Shared.DTO;

public class ApplicationRoleResponseDto
{
    public string RoleId { get; set; }
    public string Name { get; set; }
    public string NormalizedName => Name.ToUpper();
    public string Description { get; set; }
}
