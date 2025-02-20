using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Identity.Application.Features.UsersEndpoints.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    private static readonly string _resetPasswordEndpoint = $"https://localhost:7287/api/v1/Account/resetPassword";            // it's job is just to supply the resetPassword endpoint link and to that would be attached token and email which are needed by that endpoint... so when you click it, it automatically invokes the endpoint and confirms your email for you

    public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<ForgotPasswordCommandHandler> logger, IMassTransitService massTransitService)
    {
        _userManager = userManager;
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var forgotPasswordResponse = new ForgotPasswordResponse();

        var user = await _userManager.FindByEmailAsync(request.ForgotPasswordDto.Email!);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            forgotPasswordResponse.Success = false;
            forgotPasswordResponse.Message = "Bad Request";

            throw new CustomBadRequestException("Bad Request");
        }


        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        string validToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));        // you can use ASCII or UTF8

        var callbackUrl = $"{_resetPasswordEndpoint}?email={request.ForgotPasswordDto.Email!}&token={validToken}";

        //var message = new EmailMetadata(request.ForgotPasswordDto.Email!, "Forgot Password Request", $"Dear Subscriber, <br><br> If you are consuming this through a FrontEnd client, Please reset your password using the call back link by <a href={HtmlEncoder.Default.Encode(callbackUrl)}>clicking here</a>. <br><br> Else Here is your token <br><br> {HtmlEncoder.Default.Encode(validToken)} <br><br> If however, you didn't make this request, kindly ignore. <br><br> Thanks. <br><br> anointedMtc");
        //await _emailService.Send(message);

        await _massTransitService.Publish(new ForgotPasswordRequestedEvent(
            user.Email!,
            callbackUrl,
            validToken,
            user.FirstName));

        forgotPasswordResponse.Success = true;
        forgotPasswordResponse.Message = "Kindly check your email for token/link to complete the process";

        return forgotPasswordResponse;
    }
}
