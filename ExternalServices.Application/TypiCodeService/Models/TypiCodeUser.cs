namespace ExternalServices.Application.TypiCodeService.Models;

public class TypiCodeUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }



    // We override the ToString() method so we can easily display the users retrieved from the API in the console.
    public override string ToString() =>
        string.Join(Environment.NewLine, $"Id: {Id}, Name: {Name}, Email: {Email}");
}
