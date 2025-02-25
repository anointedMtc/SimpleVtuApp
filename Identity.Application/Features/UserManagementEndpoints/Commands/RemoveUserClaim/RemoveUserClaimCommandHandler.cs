using Identity.Application.Exceptions;
using Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Security.Claims;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.RemoveUserClaim;

public class RemoveUserClaimCommandHandler : IRequestHandler<RemoveUserClaimCommand, RemoveUserClaimResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RemoveUserClaimCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public RemoveUserClaimCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<RemoveUserClaimCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }
    public async Task<RemoveUserClaimResponse> Handle(RemoveUserClaimCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(RemoveUserClaimCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var removeUserClaimResponse = new RemoveUserClaimResponse();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            _logger.LogError("User with Id {UserId} not found while performing {typeOfRequest} by {AdminId}",
                request.UserId,
                typeof(GetUserClaimsQuery),
                userExecutingCommand!.Email);

            removeUserClaimResponse.Success = false;
            removeUserClaimResponse.Message = "Bad Request";

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

        foreach (var claim in request.RemoveUserClaimRequestDto.UserClaims)
        {
            if (!validOldClaims.TryGetValue(claim.Key, out string? value))
            {
                throw new CustomBadRequestException();
            }

            if (validOldClaims.TryGetValue(claim.Key, out string? valueTwo))
            {
                if (valueTwo != claim.Value)
                {
                    throw new CustomBadRequestException("The key for this claim already exists and each key-value pair must be unique");
                }
            }
        }

        List<Claim> claimsToDelete = new List<Claim>();
        foreach (var item in request.RemoveUserClaimRequestDto.UserClaims)
        {
            claimsToDelete.Add(new Claim(item.Key, item.Value));
        }
        var result = await _userManager.RemoveClaimsAsync(user, claimsToDelete);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to remove user claims for user with Id {UserId} by {AdminId}",
                user.Email,
                userExecutingCommand!.Email);

            removeUserClaimResponse.Success = false;
            removeUserClaimResponse.Message = "Failed to remove Claim. Something went wrong, please try again later.";

            throw new CustomInternalServerException();
        }

        _logger.LogInformation("Admin {AdminEmail} removed claims for User with Id {UserId}: {@Request}",
            userExecutingCommand!.Email,
            user.Email,
            request.RemoveUserClaimRequestDto.UserClaims);

        removeUserClaimResponse.Success = true;
        removeUserClaimResponse.Message = $"Successfully removed the following claims for User";


        return removeUserClaimResponse;
    }
}
