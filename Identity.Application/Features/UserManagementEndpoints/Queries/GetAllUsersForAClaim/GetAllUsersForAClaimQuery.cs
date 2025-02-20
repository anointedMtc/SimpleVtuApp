using ApplicationSharedKernel.HelperClasses;
using ApplicationSharedKernel.Interfaces;
using Identity.Application.HelperClasses;
using Identity.Shared.DTO;
using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetAllUsersForAClaim;

public class GetAllUsersForAClaimQuery : ICachedQuery<Pagination<GetAllUsersForAClaimResponse>>
{
    public GetAllUsersForAClaimQuery(PaginationFilter paginationFilterAppUser)
    {
        PaginationFilterAppUser = paginationFilterAppUser;
    }

    public PaginationFilter PaginationFilterAppUser { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetAllUsersForAClaimCacheKey(PaginationFilterAppUser);

    public TimeSpan? Expiration => null;

    public GetAllUsersForAClaimRequestDto GetAllUsersForAClaimRequestDto { get; set; }

}
