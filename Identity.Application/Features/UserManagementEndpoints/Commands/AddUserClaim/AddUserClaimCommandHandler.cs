using Identity.Application.Exceptions;
using Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Security.Claims;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.AddUserClaim;

public class AddUserClaimCommandHandler : IRequestHandler<AddUserClaimCommand, AddUserClaimResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AddUserClaimCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public AddUserClaimCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<AddUserClaimCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }
    public async Task<AddUserClaimResponse> Handle(AddUserClaimCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(AddUserClaimCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var addUserClaimResponse = new AddUserClaimResponse();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            _logger.LogError("User with Id {UserId} not found while performing {typeOfRequest} by {AdminId}",
                request.UserId,
                typeof(GetUserClaimsQuery),
                userExecutingCommand!.Email);

            addUserClaimResponse.Success = false;
            addUserClaimResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        var existingClaims = await _userManager.GetClaimsAsync(user);
        var validOldClaims = new Dictionary<string, List<string>>();
        foreach (var claim in existingClaims)
        {
            if (!validOldClaims.ContainsKey(claim.Type))
            {
                validOldClaims.Add(claim.Type.ToString(), new List<string> { claim.Value.ToString() });
            }
            else
            {
                validOldClaims[claim.Type.ToString()].Add(claim.Value.ToString());
            }
        }

        foreach (var claim in request.AddUserClaimRequestDto.UserClaims)
        {
            if (validOldClaims.TryGetValue(claim.Key, out List<string>? value))
            {
                throw new CustomBadRequestException("The key for this claim already exists and each key-value pair must be unique");
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(claim.Key.ToLower().ToString(), claim.Value.ToLower().ToString()));
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to add user claims for user with Id {UserId} by {AdminId}",
                    user.Email,
                    userExecutingCommand!.Email);

                addUserClaimResponse.Success = false;
                addUserClaimResponse.Message = "Failed to add Claim. Something went wrong, please try again later.";

                throw new CustomInternalServerException();
            }
        }

        _logger.LogInformation("Admin {AdminEmail} added claims for User with Id {UserId}: {@Request}",
            userExecutingCommand!.Email,
            user.Email,
            request.AddUserClaimRequestDto.UserClaims);

        addUserClaimResponse.Success = true;
        addUserClaimResponse.Message = "Successfully added new claims for User";

        return addUserClaimResponse;
    }
}
