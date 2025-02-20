using ApplicationSharedKernel.Interfaces;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DeleteUserCommand> _logger;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<DeleteUserCommand> logger,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Delete))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(DeleteUserCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var deleteUserResponse = new DeleteUserResponse();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            _logger.LogError("User not found");
            //throw new Exception("User not found");

            deleteUserResponse.Success = false;
            deleteUserResponse.Message = "Bad Request";

            return deleteUserResponse;
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("User update failed");

            deleteUserResponse.Success = false;
            deleteUserResponse.Message = "Failed to delete User. please try again later";

            return deleteUserResponse;
        }

        deleteUserResponse.Success = true;
        deleteUserResponse.Message = $"Successfully deleted User";

        return deleteUserResponse;

    }
}
