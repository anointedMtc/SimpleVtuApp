using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Security.Claims;

namespace Identity.Application.Features.RoleManagement.Commands.RemoveRoleClaim;

public class RemoveRoleClaimCommandHandler : IRequestHandler<RemoveRoleClaimCommand, RemoveRoleClaimResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RemoveRoleClaimCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RemoveRoleClaimCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<RemoveRoleClaimCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _roleManager = roleManager;
    }
    public async Task<RemoveRoleClaimResponse> Handle(RemoveRoleClaimCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(RemoveRoleClaimCommand),
                request);

            throw new CustomForbiddenException();
        }

        var removeRoleClaimResponse = new RemoveRoleClaimResponse();

        var roleToRemoveClaim = await _roleManager.FindByIdAsync(request.AppRoleId.ToString());

        if (roleToRemoveClaim is null)
        {
            _logger.LogWarning("User with Id {UserId} tried to add claim {Claim} to non-existing role {roleId}",
                userExecutingCommand!.Email,
                nameof(RemoveRoleClaimCommand),
                request.AppRoleId);

            throw new CustomBadRequestException();
        }

        var existingClaims = await _roleManager.GetClaimsAsync(roleToRemoveClaim);

        var validOldClaims = new Dictionary<string, string>();
        foreach (var claim in existingClaims)
        {
            if (!validOldClaims.ContainsKey(claim.Type))
            {
                validOldClaims.Add(claim.Type.ToString(), claim.Value.ToString());
            }

        }

        foreach (var claim in request.RemoveRoleClaimRequestDto.RoleClaims)
        {
            string value;
            if (validOldClaims.TryGetValue(claim.Key, out value))
            {
                throw new CustomBadRequestException("Keys do not match. Bad Request");
            }

            string valueTwo;
            if (validOldClaims.TryGetValue(claim.Key, out valueTwo))
            {
                if (valueTwo != claim.Value)
                {
                    throw new CustomBadRequestException("The key for this claim does not match the value passed");
                }
            }

            var result = await _roleManager.RemoveClaimAsync(roleToRemoveClaim, new Claim(claim.Key.ToLower().ToString(), claim.Value.ToLower().ToString()));

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to remove role claims for role with Id {RoleId} by {AdminId}",
                roleToRemoveClaim.Name,
                userExecutingCommand!.Email);

                removeRoleClaimResponse.Success = false;
                removeRoleClaimResponse.Message = "Failed to remove Claim. Something went wrong, please try again later.";

                throw new CustomInternalServerException("An Error occured while processing your request.\r\n The support team is notified and we are working on the fix. In case it's urgent, please contact us on info@anointedMtc.com");
            }
        }

        _logger.LogInformation("Admin {AdminEmail} removed claims for Role with Id {RoleId}: {@Request}",
            userExecutingCommand!.Email,
        roleToRemoveClaim.Name,
        request.RemoveRoleClaimRequestDto.RoleClaims);

        removeRoleClaimResponse.Success = true;
        removeRoleClaimResponse.Message = $"Successfully removed the following claims for User";

        return removeRoleClaimResponse;

    }
}
