using Identity.Application.Exceptions;
using Identity.Application.Features.UsersEndpoints.ChangeEmailRequest;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Text;

namespace Identity.Application.Features.UsersEndpoints.ChangeEmailConfirmation;

public class ChangeEmailConfirmationCommandHandler : IRequestHandler<ChangeEmailConfirmationCommand, ChangeEmailConfirmationResponse>
{
    private readonly ILogger<ChangeEmailConfirmationCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserContext _userContext;

    public ChangeEmailConfirmationCommandHandler(ILogger<ChangeEmailConfirmationCommandHandler> logger,
        UserManager<ApplicationUser> userManager, IUserContext userContext)
    {
        _logger = logger;
        _userManager = userManager;
        _userContext = userContext;
    }
    public async Task<ChangeEmailConfirmationResponse> Handle(ChangeEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        var changeEmailConfirmationResponse = new ChangeEmailConfirmationResponse();

        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("User with Id {UserId} requesting {typeOfOperation} from {currentEmail} to {NewEmail}",
            user!.Id,
            nameof(ChangeEmailConfirmationCommand),
            user.Email,
            request.ChangeEmailConfirmationRequestDto.NewEmail);

        var dbUser = await _userManager.FindByIdAsync(user!.Id);

        if (dbUser == null || !await _userManager.IsEmailConfirmedAsync(dbUser))
        {
            _logger.LogWarning("Non-existing user with Id {userId} tried to perform {typeOfOperation} from {currentEmail} to {NewEmail}",
            user!.Id,
            nameof(ChangeEmailRequestCommand),
            user.Email,
            request.ChangeEmailConfirmationRequestDto.NewEmail);

            throw new CustomBadRequestException();
        }

        var originalToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ChangeEmailConfirmationRequestDto.Token!));

        var confirmedResult = await _userManager.ChangeEmailAsync(dbUser, request.ChangeEmailConfirmationRequestDto.NewEmail, originalToken);
        if (!confirmedResult.Succeeded)
            throw new Exception("Invalid Email Confirmation Request");

        changeEmailConfirmationResponse.Success = true;
        changeEmailConfirmationResponse.Message = $"Email confirmed successfully, you can proceed to login";

        _logger.LogInformation("User confirmed change of their email from {currentEmail} to {newEmail} at {Time}",
            user.Email,
            request.ChangeEmailConfirmationRequestDto.NewEmail,
            DateTime.UtcNow);

        dbUser.UpdatedAt = DateTime.UtcNow;
        var result = await _userManager.UpdateAsync(dbUser);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to update user: {errors}", errors);
            throw new Exception($"Failed to update user: {errors}");
        }

        changeEmailConfirmationResponse.Success = true;
        changeEmailConfirmationResponse.Message = "successfully updated new email";

        return changeEmailConfirmationResponse;
    }
}
