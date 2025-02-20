namespace Identity.Shared.DTO;

public class RemoveUserClaimRequestDto
{
    //public Guid UserId { get; set; }
    public Dictionary<string, string> UserClaims { get; set; } = [];
    //public List<string> UserClaims { get; set; } = [];
}
