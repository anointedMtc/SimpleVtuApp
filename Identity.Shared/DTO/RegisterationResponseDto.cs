namespace Identity.Shared.DTO;

public class RegisterationResponseDto
{
    public bool IsSuccessfulRegisteration { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
