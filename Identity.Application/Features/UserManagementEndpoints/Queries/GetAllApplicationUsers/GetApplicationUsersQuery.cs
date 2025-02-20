using ApplicationSharedKernel.HelperClasses;
using ApplicationSharedKernel.Interfaces;
using Identity.Application.HelperClasses;
using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllApplicationUsers;

public class GetApplicationUsersQuery : ICachedQuery<Pagination<GetApplicationUsersResponse>>
{
    public GetApplicationUsersQuery(PaginationFilter paginationFilterAppUser) : base()
    {
        PaginationFilterAppUser = paginationFilterAppUser;
    }

    public PaginationFilter PaginationFilterAppUser { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetAllApplicationUsersCacheKey(PaginationFilterAppUser);

    public TimeSpan? Expiration => null;
}
