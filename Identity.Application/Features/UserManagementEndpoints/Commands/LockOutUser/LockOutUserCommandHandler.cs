using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.LockOutUser;

public class LockOutUserCommandHandler : IRequestHandler<LockOutUserCommand, LockOutUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<LockOutUserCommandHandler> _logger;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public LockOutUserCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<LockOutUserCommandHandler> logger,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _userManager = userManager;
        _logger = logger;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<LockOutUserResponse> Handle(LockOutUserCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.LockOut))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(LockOutUserCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }


        var lockOutUserResponse = new LockOutUserResponse();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            _logger.LogError("User not found");

            lockOutUserResponse.Success = false;
            lockOutUserResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        // you can decide to check...
        //if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
        user.LockoutEnd = DateTime.Now.AddYears(1000);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("User LockOut failed");

            lockOutUserResponse.Success = false;
            lockOutUserResponse.Message = "Failed to lockout user. please try again later";

            throw new CustomInternalServerException();
        }

        lockOutUserResponse.Success = true;
        lockOutUserResponse.Message = $"Successfully LockedOut User";

        return lockOutUserResponse;
    }
}
