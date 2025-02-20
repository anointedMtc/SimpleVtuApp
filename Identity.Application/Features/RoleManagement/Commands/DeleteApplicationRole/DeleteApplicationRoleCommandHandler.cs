using ApplicationSharedKernel.Interfaces;
using AutoMapper;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.RoleManagement.Commands.DeleteApplicationRole;

public class DeleteApplicationRoleCommandHandler : IRequestHandler<DeleteApplicationRoleCommand, DeleteApplicationRoleResponse>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<DeleteApplicationRoleCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteApplicationRoleCommandHandler(RoleManager<ApplicationRole> roleManager,
        ILogger<DeleteApplicationRoleCommandHandler> logger, IMapper mapper,
        IUserContext userContext, IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userManager = userManager;
    }
    public async Task<DeleteApplicationRoleResponse> Handle(DeleteApplicationRoleCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Delete))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(DeleteApplicationRoleCommand),
                request);

            throw new CustomForbiddenException();
        }

        var deleteApplicationRoleResponse = new DeleteApplicationRoleResponse();

        var existingRole = await _roleManager.FindByIdAsync(request.RoleId.ToString());

        if (existingRole == null)
        {
            _logger.LogError("Role {Role} does not exist", request.RoleId);
            deleteApplicationRoleResponse.Success = false;
            deleteApplicationRoleResponse.Message = "Bad Request";

            throw new CustomBadRequestException("Bad Request");
        }

        //var userAssignedToThisRole = _roleManager.Roles.Where(u => u.Id == request.RoleId.ToString()).Count();

        var userAssignedToThisRole = await _userManager.GetUsersInRoleAsync(existingRole.Name!);

        if (userAssignedToThisRole.Count > 0)
        {
            _logger.LogError("Cannot delete this role {Role} since there are users assigned to it",
                existingRole.Name);

            deleteApplicationRoleResponse.Success = false;
            deleteApplicationRoleResponse.Message = "Cannot delete this role {Role} since there are users assigned to it";

            throw new CustomBadRequestException("Bad Request. Users assigned to this role");
        }

        var result = await _roleManager.DeleteAsync(existingRole);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to delete role {Role}", existingRole.Name);

            deleteApplicationRoleResponse.Success = false;
            deleteApplicationRoleResponse.Message = "Failed to delete role. Something went wrong";

            throw new CustomInternalServerException("An Error occured while processing your request.\r\n The support team is notified and we are working on the fix. In case it's urgent, please contact us on info@anointedMtc.com");
        }

        deleteApplicationRoleResponse.Success = true;
        deleteApplicationRoleResponse.Message = "Successfully deleted ApplicationRole";

        return deleteApplicationRoleResponse;
    }
}
