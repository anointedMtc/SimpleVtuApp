using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.UsersEndpoints.DisableOrEnableTwoFacAuth;

public class DisableOrEnableTwoFacAuthCommandHandler : IRequestHandler<DisableOrEnableTwoFacAuthCommand, DisableOrEnableTwoFacAuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DisableOrEnableTwoFacAuthCommandHandler> _logger;
    private readonly IUserContext _userContext;

    public DisableOrEnableTwoFacAuthCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<DisableOrEnableTwoFacAuthCommandHandler> logger, IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
    }

    public async Task<DisableOrEnableTwoFacAuthResponse> Handle(DisableOrEnableTwoFacAuthCommand request, CancellationToken cancellationToken)
    {
        // this is the endpoint individual users should hit to update their details by themselves... since it makes use of context to automatically get the user... it is assumed it would be a link on the page or somewhere... 
        var disableOrEnableTwoFacAuthResponse = new DisableOrEnableTwoFacAuthResponse();

        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("Disabling TwoFacAuth for usew with Id {UserId}", user.Email);

        var dbUser = await _userManager.FindByIdAsync(user!.Id);

        if (dbUser == null)
        {
            _logger.LogWarning("Non existing User with Id {UserId} tried to perform {typeOfReqeust}",
                user.Email, nameof(DisableOrEnableTwoFacAuthCommand));

            disableOrEnableTwoFacAuthResponse.Success = false;
            disableOrEnableTwoFacAuthResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(dbUser, request.IsTwoFacAuthEnabled);

        if (!disable2faResult.Succeeded)
        {
            _logger.LogWarning("Failed to disable 2fa for User with Id {UserId}",
                dbUser.Email);

            disableOrEnableTwoFacAuthResponse.Success = false;
            disableOrEnableTwoFacAuthResponse.Message = "Something went wrong. Please try again later";

            throw new CustomInternalServerException("Something went wrong. Please try again after some time");
        }

        _logger.LogInformation("User with ID {UserId} has disabled 2fa.", dbUser.Email);
        disableOrEnableTwoFacAuthResponse.Success = true;
        disableOrEnableTwoFacAuthResponse.Message = "Successfully disabled 2fa. You can reenable 2fa when you setup an authenticator app";

        return disableOrEnableTwoFacAuthResponse;
    }
}
