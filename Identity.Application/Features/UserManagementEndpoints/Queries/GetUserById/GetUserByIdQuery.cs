using Identity.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserById;

public class GetUserByIdQuery : ICachedQuery<GetUserByIdResponse>
{
    public Guid UserId { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetUserByIdQueryCacheKey(UserId);

    public TimeSpan? Expiration => null;
}
