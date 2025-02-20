namespace Identity.Shared.DTO;

public class AddApplicationRoleRequestDto
{
    public string Name { get; set; }
    public string NormalizedName => Name.ToUpper();
    public string Description { get; set; }
}

