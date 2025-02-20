namespace Identity.Shared.DTO;

public class UpdateUserClaimRequestDto
{
    //public Guid UserId { get; set; }
    public Dictionary<string, string> UserClaims { get; set; } = [];
    //public List<string> UserClaims { get; set; } = [];

}
