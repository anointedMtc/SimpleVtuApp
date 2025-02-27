using System.ComponentModel.DataAnnotations;

namespace Identity.Shared.DTO;

public class ApplicationUserRegisterationRequestDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string Gender { get; set; }
    public DateOfBirthResponseDto DateOfBirth { get; set; }

    public string? Nationality { get; set; }

    // since the EmailService would be included here the client app needs to provide a Uri where the user would be navigated to see the confirmation message about the verification flow... for example you would see something like... Email Confirmation (as heading) Your email has been successfully confirmed. Please click here to log in (uri). Also the page would be used to process the email verification logic
    // public string? ClientUri { get; set; }      // https://localhost:7023/api/Account/emailconfirmation   this is the endpoint of emailconfirmation and should be authomatically populated/filled-in by the client/browser... the question though is should it be here? it can be passed in from the handler/service/method handling the generation of callback link... because it's job is just to supply the emailConfirmation endpoint link and to that would be attached token and email which are needed by that endpoint

    public bool IsTwoFacAuthEnabled { get; set; }
}
