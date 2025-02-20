namespace Notification.Shared.DTO;

public class UserEmailDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? MemberType { get; set; }

    public UserEmailDto(string name, string email, string? memberType)
    {
        Name = name;
        Email = email;
        MemberType = memberType;
    }
}
