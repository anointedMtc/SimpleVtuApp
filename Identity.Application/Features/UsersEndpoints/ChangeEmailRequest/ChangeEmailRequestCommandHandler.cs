using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using Identity.Domain.Entities;
using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Shared.IntegrationEvents;

namespace Identity.Application.Features.UsersEndpoints.ChangeEmailRequest;

public class ChangeEmailRequestCommandHandler : IRequestHandler<ChangeEmailRequestCommand, ChangeEmailRequestResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ChangeEmailRequestCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IMassTransitService _massTransitService;

    private static readonly string _resetEmailEndpoint = $"https://localhost:7287/api/v1/Account/confirm-change-email";            // it's job is just to supply the resetPassword endpoint link and to that would be attached token and email which are needed by that endpoint... so when you click it, it automatically invokes the endpoint and confirms your email for you

    public ChangeEmailRequestCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<ChangeEmailRequestCommandHandler> logger,
        IUserContext userContext, IMassTransitService massTransitService)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _massTransitService = massTransitService;
    }
    public async Task<ChangeEmailRequestResponse> Handle(ChangeEmailRequestCommand request, CancellationToken cancellationToken)
    {
        // you must first of all be loggedIn in order to access this endpoint... this is because if someone is sure that an Email is valid, but can't get access to the account, he/she can request a change of email to the one he/she can acess which is bad... so YOU MUST BE LOGGED IN TO EVER ACCESS THIS ENDPOINT

        var changeEmailRequestResponse = new ChangeEmailRequestResponse();

        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("User with Id {UserId} requesting {typeOfOperation} from {currentEmail} to {NewEmail}",
            user!.Id,
            nameof(ChangeEmailRequestCommand),
            user.Email,
            request.ChangeEmailRequestDto.NewEmail);

        var dbUser = await _userManager.FindByIdAsync(user!.Id);

        if (dbUser == null || !await _userManager.IsEmailConfirmedAsync(dbUser))
        {
            _logger.LogWarning("Non-existing user with Id {userId} tried to perform {typeOfOperation} from {currentEmail} to {NewEmail}",
            user!.Id,
            nameof(ChangeEmailRequestCommand),
            user.Email,
            request.ChangeEmailRequestDto.NewEmail);

            throw new CustomBadRequestException();
        }
        

        if (dbUser.Email == request.ChangeEmailRequestDto.NewEmail)
        {
            _logger.LogError("user with Id {UserId} tried to perform {typeOfOperation} using the same email {MailId}",
                user.Id,
                nameof(ChangeEmailRequestCommand),
                dbUser.Email);

            changeEmailRequestResponse.Success = false;
            changeEmailRequestResponse.Message = "Can't change email with already existing one";

            throw new CustomBadRequestException();
        }

        var resetToken = await _userManager.GenerateChangeEmailTokenAsync(dbUser, request.ChangeEmailRequestDto.NewEmail);

        var validToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

        var callbackUrl = $"{_resetEmailEndpoint}?newEmail={request.ChangeEmailRequestDto.NewEmail!}&currentEmail={user.Email}&token={validToken}";

        
        //var message = new EmailMetadata(request.ChangeEmailRequestDto.NewEmail!, "Change Email Request Token", $"Dear Subscriber, <br><br> If you are consuming this through a FrontEnd client, Please confirm the change of email request by using the call back link by <a href={HtmlEncoder.Default.Encode(callbackUrl)}>clicking here</a>. <br><br> Else Here is your token <br><br> {HtmlEncoder.Default.Encode(validToken)} <br><br> If however, you didn't make this request, kindly ignore. <br><br> Thanks. <br><br> anointedMtc");
        //await _emailService.Send(message);
        // ALSO SEND AN EMAIL TO THE CURRENT EMAIL INFORMING THEM OF THE CHANGE-EMAIL REQUEST
        //var messageTwo = new EmailMetadata(user.Email, "Change of Email Request", $"Dear Subscriber, <br><br> A request was made to change your email with us from this to another email. If it wasn't you, kindly reach out to us at <br><br> info@anointedMtc.com   <br><br>  Thanks.  <br><br> anointedMtc");

        await _massTransitService.Publish(new ChangeEmailRequestedEvent(
            user.Email,
            request.ChangeEmailRequestDto.NewEmail,
            callbackUrl,
            validToken,
            user.FirstName!)
        );

        changeEmailRequestResponse.Success = true;
        changeEmailRequestResponse.Message = "Confirmation link to change email sent. Please check your email for token/link to complete the process.";

        return changeEmailRequestResponse;
    }
}
