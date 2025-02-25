using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Text;
using System.Text.Encodings.Web;

namespace Identity.Application.Features.UsersEndpoints.ResendEmailConfirmation;

public class ResendEmailConfirmationCommandHandler : IRequestHandler<ResendEmailConfirmationCommand, ResendEmailConfirmationResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ResendEmailConfirmationCommandHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    private static readonly string _emailConfirmationEndpoint = $"https://localhost:7287/api/v1/Account/emailconfirmation";            // it's job is just to supply the emailConfirmation endpoint link and to that would be attached token and email which are needed by that endpoint... so when you click it, it automatically invokes the endpoint and confirms your email for you

    public ResendEmailConfirmationCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<ResendEmailConfirmationCommandHandler> logger,
        IMassTransitService massTransitService)
    {
        _userManager = userManager;
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task<ResendEmailConfirmationResponse> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var resendEmailConfirmationResponse = new ResendEmailConfirmationResponse();

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || await _userManager.IsEmailConfirmedAsync(user))
        {
            // we don't want to let them know that the user wasn't found
            _logger.LogWarning("Non Existing User with {EmailId} requesting for {typeOfRequest}",
                request.Email,
                typeof(ResendEmailConfirmationCommand));

            resendEmailConfirmationResponse.Success = true;
            resendEmailConfirmationResponse.Message = "Verification email sent. Please check our email.";

            //return resendEmailConfirmationResponse;

            throw new CustomBadRequestException("Bad Request");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        _logger.LogInformation("EmailConfirmationToken generated for {User} at {Time}",
            request.Email, DateTime.UtcNow);

        var validToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var callbackUrl = $"{_emailConfirmationEndpoint}?email={request.Email}&token={validToken}";

        //var message = new EmailMetadata(request.Email!, "Email Confirmation Token", $"Dear Subscriber, <br><br>Please confirm your Email account by <a href={HtmlEncoder.Default.Encode(callbackUrl)}>clicking here</a>.  <br><br> You can as well choose to copy your Token below and paste in appropriate apiEndpoint: <br><br> {HtmlEncoder.Default.Encode(validToken)} <br><br> If however you didn't make this request, kindly ignore. <br><br> Thanks <br><br> anointedMtc");
        //await _emailService.Send(message);

        await _massTransitService.Publish(new ResendEmailConfirmationRequestedEvent(
            user.Email!,
            user.FirstName,
            callbackUrl,
            validToken));

        resendEmailConfirmationResponse.Success = true;
        resendEmailConfirmationResponse.Message = "Verification email sent. Please check our email.";

        return resendEmailConfirmationResponse;

    }
}
