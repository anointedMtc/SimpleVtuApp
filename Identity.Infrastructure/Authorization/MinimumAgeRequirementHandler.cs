using ApplicationSharedKernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Authorization;

internal class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
    IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        // assuming we didn't have the IUserContext service class, you could access the date of birth like this... 
        //var dob = context.User.Claims.FirstOrDefault(c => c.Type == "");

        logger.LogInformation("User: {Email}, date of birth {DoB} - Handling MinimumAgeRequirement",
            currentUser.Email,
            currentUser.DateOfBirth);

        if (currentUser.DateOfBirth == null)
        {
            logger.LogWarning("User date of birth is null");
            context.Fail();
            return Task.CompletedTask;
        }
        // if that value is less or equal to DateTime.Today, which we would have to convert to DateOnly value by doing/saying/casting with DateOnly.FromDateTime()                                        
        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
        {
            // if the above is true, then this should mean that our User minimum age requirement is properly handled
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
