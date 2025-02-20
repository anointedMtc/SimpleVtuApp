namespace Identity.Shared.DTO;

public class GetUserClaimsResponseDto
{
    public string UserId { get; set; }
    public string Email { get; set; }
    //public List<string>? Roles { get; set; }
    public IEnumerable<string>? Roles { get; set; }
    //public Dictionary<string, string> UserClaims { get; set; } = [];
    public Dictionary<string, List<string>> UserClaims { get; set; } = [];


    //public List<string> UserClaims { get; set; } = [];

}
