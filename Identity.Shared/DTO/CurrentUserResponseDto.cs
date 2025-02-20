namespace Identity.Shared.DTO;

public class CurrentUserResponseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public string AccessToken { get; set; }    // to make it fun but not something you may do in production 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime LastLogin { get; set; } = DateTime.Now;

}
