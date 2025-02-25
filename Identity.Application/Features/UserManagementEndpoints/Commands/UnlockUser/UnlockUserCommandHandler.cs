using Identity.Application.Exceptions;
using Identity.Application.Features.UserManagementEndpoints.Commands.LockOutUser;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.UnlockUser;

public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, UnlockUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UnlockUserCommandHandler> _logger;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public UnlockUserCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<UnlockUserCommandHandler> logger,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }
    public async Task<UnlockUserResponse> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.Unlock))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(LockOutUserCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }


        var unlockUserResponse = new UnlockUserResponse();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            _logger.LogError("User not found");

            unlockUserResponse.Success = false;
            unlockUserResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        user.LockoutEnd = DateTime.Now;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("Failed to unlock user");

            unlockUserResponse.Success = false;
            unlockUserResponse.Message = "Failed to unlock user. please try again later";

            throw new CustomInternalServerException();
        }

        unlockUserResponse.Success = true;
        unlockUserResponse.Message = $"Successfully unlocked User";

        return unlockUserResponse;
    }
}
