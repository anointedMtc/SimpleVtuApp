using ApplicationSharedKernel.Interfaces;
using Identity.Shared.Constants;
using Microsoft.Extensions.Logging;

namespace ApplicationSharedKernel.Services;

public class ResourceBaseAuthorizationService : IResourceBaseAuthorizationService
{
    private readonly ILogger _logger;
    private readonly IUserContext _userContext;

    public ResourceBaseAuthorizationService(ILogger<ResourceBaseAuthorizationService> logger, IUserContext userContext)
    {
        _logger = logger;
        _userContext = userContext;
    }

    //public bool Authorize(ResourceOperation resourceOperation)
    public bool Authorize(string resourceOperation)
    {
        var user = _userContext.GetCurrentUser();

        // if user is null, no need to even go further...
        if (user is null)
        {
            _logger.LogWarning("Non User tried to access sensitive data");
            return false;
        }

        _logger.LogInformation("Authorizing user {UserEmail}, to {@Operation}",
            user.Email,
            resourceOperation);

        if (resourceOperation.Equals(ResourceOperation.Read, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Read operation - successful authorization");
            return true;
        }

        if ((resourceOperation.Equals(ResourceOperation.Assign, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.UnAssign, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.LockOut, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.Unlock, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.AdminAndAbove, StringComparison.OrdinalIgnoreCase))
            && user.IsInRole(AppUserRoles.Admin))
        {
            _logger.LogInformation("Admin user, delete operation - successful authorization");
            return true;
        }

        if ((resourceOperation.Equals(ResourceOperation.Create, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.Update, StringComparison.OrdinalIgnoreCase)  || 
            resourceOperation.Equals(ResourceOperation.Delete, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.Assign, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.UnAssign, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.LockOut, StringComparison.OrdinalIgnoreCase)  || 
            resourceOperation.Equals(ResourceOperation.Unlock, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.AdminAndAbove, StringComparison.OrdinalIgnoreCase) || 
            resourceOperation.Equals(ResourceOperation.GodsEyeOnly, StringComparison.OrdinalIgnoreCase))
            && user.IsInRole(AppUserRoles.GodsEye))
        {
            _logger.LogInformation("GodsEye - successful authorization");
            return true;
        }

        _logger.LogWarning("{User} tried to access a forbidden resource",
            user.Email);

        return false;
    }
}
