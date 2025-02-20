using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Identity.Application.Features.RoleManagement.Commands.AddRoleClaim;

public class AddRoleClaimCommandHandler : IRequestHandler<AddRoleClaimCommand, AddRoleClaimResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AddRoleClaimCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AddRoleClaimCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<AddRoleClaimCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _roleManager = roleManager;
    }
    public async Task<AddRoleClaimResponse> Handle(AddRoleClaimCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(AddRoleClaimCommand),
                request);

            throw new CustomForbiddenException("You are not allowed to access this command");
        }

        var addRoleClaimResponse = new AddRoleClaimResponse();

        var roleToAddClaim = await _roleManager.FindByIdAsync(request.AppRoleId.ToString());

        if (roleToAddClaim is null)
        {
            _logger.LogWarning("User with Id {UserId} tried to add claim {Claim} to non-existing role {roleId}",
                userExecutingCommand!.Email,
                nameof(AddRoleClaimCommand),
                request.AppRoleId);

            throw new CustomBadRequestException("Bad request");
        }

        
        foreach (var claim in request.AddRoleClaimRequestDto.RoleClaims)
        {
            var result = await _roleManager.AddClaimAsync(roleToAddClaim, new Claim(claim.Key.ToLower().ToString(), claim.Value.ToLower().ToString()));
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to add role claims for role with Id {RoleId} by {AdminId}",
                    roleToAddClaim.Name,
                    userExecutingCommand!.Email);

                addRoleClaimResponse.Success = false;
                addRoleClaimResponse.Message = "Failed to add Claim. Something went wrong, please try again later.";

                throw new CustomInternalServerException("An Error occured while processing your request.\r\n The support team is notified and we are working on the fix. In case it's urgent, please contact us on info@anointedMtc.com");
            }
        }

        _logger.LogInformation("Admin {AdminEmail} added claims for Role with Id {RoleId}: {@Request}",
            userExecutingCommand!.Email,
            roleToAddClaim.Name,
            request.AddRoleClaimRequestDto.RoleClaims);

        addRoleClaimResponse.Success = true;
        addRoleClaimResponse.Message = "Successfully added new claims for User";

        return addRoleClaimResponse;
    }
}




