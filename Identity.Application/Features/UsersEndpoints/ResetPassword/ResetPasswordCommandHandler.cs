using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Identity.Application.Features.UsersEndpoints.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var resetPasswordResponse = new ResetPasswordResponse();


        var user = await _userManager.FindByEmailAsync(request.ResetPasswordDto.Email!);
        if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            resetPasswordResponse.Success = false;
            resetPasswordResponse.Message = "Bad Request";

            throw new CustomBadRequestException("Bad Request");
        }

        var originalToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetPasswordDto.Token!));

        var result = await _userManager.ResetPasswordAsync(user, originalToken, request.ResetPasswordDto.NewPassword!);
        if (!result.Succeeded)
        {
            resetPasswordResponse.Success = false;
            resetPasswordResponse.Message = "Bad Request";

            throw new CustomInternalServerException("Something went wrong. Please try again after some time");
        }


        resetPasswordResponse.Success = true;
        resetPasswordResponse.Message = "Password reset was successful";

        await _userManager.SetLockoutEndDateAsync(user, null); // setting a date in the past immediately locks out the user

        return resetPasswordResponse;
    }
}
