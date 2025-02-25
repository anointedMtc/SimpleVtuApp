using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Application.Features.UserManagementEndpoints.Commands.RevokeRefreshToken;

public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, RevokeRefreshTokenResponse>
{
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IResourceBaseAuthorizationService _resourceBaseAuthorizationService;
    private readonly IUserContext _userContext;

    public RevokeRefreshTokenCommandHandler(ILogger<RevokeRefreshTokenCommandHandler> logger,
        UserManager<ApplicationUser> userManager,
        IResourceBaseAuthorizationService resourceBaseAuthorizationService,
        IUserContext userContext)
    {
        _logger = logger;
        _userManager = userManager;
        _resourceBaseAuthorizationService = resourceBaseAuthorizationService;
        _userContext = userContext;
    }

    public async Task<RevokeRefreshTokenResponse> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (!_resourceBaseAuthorizationService.Authorize(ResourceOperation.AdminAndAbove))
        {
            var userExecutingCommand = _userContext.GetCurrentUser();
            _logger.LogWarning("User {UserId} tried to access a forbidden resource {Resource} with request {@Request}",
                userExecutingCommand!.Email,
                typeof(RevokeRefreshTokenCommand),
                request);

            throw new CustomForbiddenException("Access Denied. You do not have Permission to view this resource");
        }

        var revokeRefreshTokenResponse = new RevokeRefreshTokenResponse();

        _logger.LogInformation("Revoking refresh token");

        using var sha256 = SHA256.Create();
        var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.RevokeRefreshTokenRequestDto.RefreshToken));
        var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);

        var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == hashedRefreshToken);
        if (user == null)
        {
            _logger.LogError("Invalid refresh token");
            throw new Exception("Invalid refresh token");
        }

        // validate the refresh token expiry time       -- if it has expired, then no need to revoke/delete it since the user can't do anything with it... 
        // Or maybe another implementation is instead of throwing a new exception, you just simply return successful from here... 
        if (user.RefreshTokenExpiryTime < DateTime.Now)
        {
            _logger.LogWarning("Refresh token already expired for user ID: {userId}", user.Id);
            //throw new Exception("Refresh token expired");

            _logger.LogInformation("Refresh token revoked successfully");
            revokeRefreshTokenResponse.Success = true;
            revokeRefreshTokenResponse.Message = "Refresh Token revoked successuflly";

            return revokeRefreshTokenResponse;
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError("Fialed to update user");
            revokeRefreshTokenResponse.Success = false;
            revokeRefreshTokenResponse.Message = "Fialed to revoke refresh token";

        }
        _logger.LogInformation("Refresh token revoked successfully");
        revokeRefreshTokenResponse.Success = true;
        revokeRefreshTokenResponse.Message = "Refresh Token revoked successuflly";

        return revokeRefreshTokenResponse;
    }
}
