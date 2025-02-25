using Identity.Application.HelperClasses;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersInARole;

public class GetAllUsersInARoleQuery : ICachedQuery<Pagination<GetAllUsersInARoleResponse>>
{
    public GetAllUsersInARoleQuery(PaginationFilter paginationFilterAppUser) : base()
    {
        PaginationFilterAppUser = paginationFilterAppUser;
    }

    public PaginationFilter PaginationFilterAppUser { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetAllUsersInARoleCacheKey(PaginationFilterAppUser);

    public TimeSpan? Expiration => null;
}
