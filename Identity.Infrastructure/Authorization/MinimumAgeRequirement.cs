using Microsoft.AspNetCore.Authorization;

namespace Identity.Infrastructure.Authorization;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}
