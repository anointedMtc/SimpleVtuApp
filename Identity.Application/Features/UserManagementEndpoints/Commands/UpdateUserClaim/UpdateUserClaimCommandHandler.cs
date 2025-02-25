using Identity.Application.Exceptions;
using Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Security.Claims;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UpdateUserClaim;

public class UpdateUserClaimCommandHandler : IRequestHandler<UpdateUserClaimCommand, UpdateUserClaimResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UpdateUserClaimCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public UpdateUserClaimCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<UpdateUserClaimCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }
    public async Task<UpdateUserClaimResponse> Handle(UpdateUserClaimCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(UpdateUserClaimCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var updateUserClaimResponse = new UpdateUserClaimResponse();
        var missingClaims = new Dictionary<string, string>();
        var existingClaims = new Dictionary<string, string>();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            _logger.LogError("User with Id {UserId} not found while performing {typeOfRequest} by {AdminId}",
                request.UserId,
                typeof(GetUserClaimsQuery),
                userExecutingCommand!.Email);

            updateUserClaimResponse.Success = false;
            updateUserClaimResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        var oldClaims = await _userManager.GetClaimsAsync(user);

        var validOldClaims = new Dictionary<string, string>();
        foreach (var claim in oldClaims)
        {
            if (!validOldClaims.ContainsKey(claim.Type))
            {
                validOldClaims.Add(claim.Type.ToString(), claim.Value.ToString());
            }
        }

        foreach (var claim in request.UpdateUserClaimRequestDto.UserClaims)
        {
            if (!validOldClaims.TryGetValue(claim.Key, out string? value))
            {
                throw new CustomBadRequestException();
            }

            if (validOldClaims.TryGetValue(claim.Key, out string? valueTwo))
            {
                var result = await _userManager.ReplaceClaimAsync(user, new Claim(claim.Key, valueTwo), new Claim(claim.Key, claim.Value));

                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to remove user claims for user with Id {UserId} by {AdminId}",
                        user.Email,
                        userExecutingCommand!.Email);

                    updateUserClaimResponse.Success = false;
                    updateUserClaimResponse.Message = "Failed to remove Claim. Something went wrong, please try again later.";

                    throw new CustomInternalServerException();
                }
            }
        }

        _logger.LogInformation("Admin {AdminEmail} added claims for User with Id {UserId}: {@Request}",
            userExecutingCommand!.Email,
            user.Email,
            request.UpdateUserClaimRequestDto.UserClaims);

        updateUserClaimResponse.Success = true;
        updateUserClaimResponse.Message = $"Successuflly Updated the following claims for User";

        return updateUserClaimResponse;
    }

}
