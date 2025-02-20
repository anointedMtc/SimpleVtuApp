using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UnAssignUserRole;

public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> _logger,
    UserManager<ApplicationUser> _userManager,
    RoleManager<ApplicationRole> _roleManager,
    IResourceBaseAuthorizationService _resourceBaseAuthorizationService,
    IUserContext _userContext) : IRequestHandler<UnassignUserRoleCommand, UnassignUserRoleResponse>
{
    public async Task<UnassignUserRoleResponse> Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.UnAssign))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(UnassignUserRoleCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var unassignUserRoleResponse = new UnassignUserRoleResponse();

        _logger.LogInformation("Unassigning user role: {@Request}", request);
        var user = await _userManager.FindByEmailAsync(request.UnassignUserRoleRequestDto.UserEmail)
            ?? throw new CustomNotFoundException(nameof(ApplicationUser), request.UnassignUserRoleRequestDto.UserEmail);    // here you are just passing in User as a string

        var role = await _roleManager.FindByNameAsync(request.UnassignUserRoleRequestDto.RoleName)
            ?? throw new CustomNotFoundException(nameof(ApplicationRole), request.UnassignUserRoleRequestDto.RoleName);     // here you are passing IdentityRole as a string

        // necessary to check if the user was in that role??? - looks like additional and unnecessary computing power... but maybe it could be cool... but just like in deleting resource, we don't check if the resource exists or not... we just want to make sure after the operation, the resource no longer exists...
        var result = await _userManager.RemoveFromRoleAsync(user, role.Name!);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to remove User {UserId} from Role {RoleName}",
                request.UnassignUserRoleRequestDto.UserEmail,
                request.UnassignUserRoleRequestDto.RoleName);

            unassignUserRoleResponse.Success = false;
            unassignUserRoleResponse.Message = $"Failed to remove User from Role. Please try again.";

            return unassignUserRoleResponse;
        }

        _logger.LogInformation("Successfully removed User {UserId} from Role {RoleName}",
            request.UnassignUserRoleRequestDto.UserEmail,
            request.UnassignUserRoleRequestDto.RoleName);

        unassignUserRoleResponse.Success = true;
        unassignUserRoleResponse.Message = $"Successfully removed user from {request.UnassignUserRoleRequestDto.RoleName} role";

        // raise an event here


        return unassignUserRoleResponse;

    }
}
