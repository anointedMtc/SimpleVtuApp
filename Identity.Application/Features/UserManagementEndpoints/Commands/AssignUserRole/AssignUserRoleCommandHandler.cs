using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.AssignUserRole;


public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> _logger,
    UserManager<ApplicationUser> _userManager,
    RoleManager<ApplicationRole> _roleManager,
    IResourceBaseAuthorizationService _resourceBaseAuthorizationService,
    IUserContext _userContext) : IRequestHandler<AssignUserRoleCommand, AssignUserRoleResponse>
{
    public async Task<AssignUserRoleResponse> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Assign))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(AssignUserRoleCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var assignUserRoleResponse = new AssignUserRoleResponse();

        _logger.LogInformation("Assigning user role: {@Request}", request);
        var user = await _userManager.FindByEmailAsync(request.AssignUserRoleRequestDto.UserEmail)
            ?? throw new CustomNotFoundException(nameof(ApplicationUser), request.AssignUserRoleRequestDto.UserEmail);            // here you are just passing in User as a string

        var role = await _roleManager.FindByNameAsync(request.AssignUserRoleRequestDto.RoleName)
            ?? throw new CustomNotFoundException(nameof(ApplicationRole), request.AssignUserRoleRequestDto.RoleName);      // here you are passing IdentityRole as a string


        var result = await _userManager.AddToRoleAsync(user, role.Name!);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to assign new role {Role} to User {UserEmail}",
                request.AssignUserRoleRequestDto.RoleName,
                request.AssignUserRoleRequestDto.UserEmail);

            assignUserRoleResponse.Success = false;
            assignUserRoleResponse.Message = $"Failed to assign new role to user. Please try again";

            return assignUserRoleResponse;
        }

        _logger.LogInformation("Successfully assigned new role {Role} to User {UserEmail}",
            request.AssignUserRoleRequestDto.RoleName,
            request.AssignUserRoleRequestDto.UserEmail);

        assignUserRoleResponse.Success = true;
        assignUserRoleResponse.Message = $"Successfully assigned new role of {request.AssignUserRoleRequestDto.RoleName} to User";


        return assignUserRoleResponse;

    }
}
