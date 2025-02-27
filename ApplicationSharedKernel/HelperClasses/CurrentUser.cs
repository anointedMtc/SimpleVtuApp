namespace SharedKernel.Application.HelperClasses;

public record CurrentUser(string Id,
    string? UserName,
    string Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string Gender,
    IEnumerable<string> Roles,
    string? Nationality,
    DateOnly? DateOfBirth,
    DateTime? CreatedAt,
    DateTime? LastLogin
    )
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
