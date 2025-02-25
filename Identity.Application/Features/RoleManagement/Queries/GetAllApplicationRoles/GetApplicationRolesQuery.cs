using Identity.Application.HelperClasses;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.Features.RoleManagement.Queries.GetAllApplicationRoles;

public class GetApplicationRolesQuery : ICachedQuery<Pagination<GetApplicationRolesResponse>>
{
    public GetApplicationRolesQuery(PaginationFilter paginationFilterAppUser) : base()
    {
        PaginationFilterAppUser = paginationFilterAppUser;
    }

    public PaginationFilter PaginationFilterAppUser { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetAllApplicationRolesCacheKey(PaginationFilterAppUser);

    public TimeSpan? Expiration => null;

}
