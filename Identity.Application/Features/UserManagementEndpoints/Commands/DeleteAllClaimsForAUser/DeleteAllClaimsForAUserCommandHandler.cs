using Identity.Application.Exceptions;
using Identity.Application.Features.UserManagementEndpoints.Queries.GetUserClaims;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.DeleteAllClaimsForAUser;

public class DeleteAllClaimsForAUserCommandHandler : IRequestHandler<DeleteAllClaimsForAUserCommand, DeleteAllClaimsForAUserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DeleteAllClaimsForAUserCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;

    public DeleteAllClaimsForAUserCommandHandler(UserManager<ApplicationUser> userManager,
        ILogger<DeleteAllClaimsForAUserCommandHandler> logger, IUserContext userContext,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService)
    {
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
    }
    public async Task<DeleteAllClaimsForAUserResponse> Handle(DeleteAllClaimsForAUserCommand request, CancellationToken cancellationToken)
    {
        var userExecutingCommand = _userContext.GetCurrentUser();
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(DeleteAllClaimsForAUserCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var deleteAllClaimsForAUserResponse = new DeleteAllClaimsForAUserResponse();

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            _logger.LogError("User with Id {UserId} not found while performing {typeOfRequest} by {AdminId}",
                request.UserId,
                typeof(GetUserClaimsQuery),
                userExecutingCommand!.Email);

            deleteAllClaimsForAUserResponse.Success = false;
            deleteAllClaimsForAUserResponse.Message = "Bad Request";

            throw new CustomBadRequestException();
        }

        var oldClaims = await _userManager.GetClaimsAsync(user);

        var result = await _userManager.RemoveClaimsAsync(user, oldClaims);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to remove user claims for user with Id {UserId} by {AdminId}",
                user.Email,
                userExecutingCommand!.Email);

            deleteAllClaimsForAUserResponse.Success = false;
            deleteAllClaimsForAUserResponse.Message = "Failed to remove Claim. Something went wrong, please try again later.";

            throw new CustomInternalServerException();
        }

        _logger.LogInformation("Admin {AdminEmail} removed all claims for User with Id {UserId}",
            userExecutingCommand!.Email,
            user.Email);

        deleteAllClaimsForAUserResponse.Success = true;
        deleteAllClaimsForAUserResponse.Message = $"Successfully removed all claims for User";


        return deleteAllClaimsForAUserResponse;
    }
}
