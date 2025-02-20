using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.HelperClasses
{
    public static class CacheHelpers
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
        private static readonly string _applicationRolesKeyTemplate = "roles-{0}-{1}-{2}-{3}";
        private static readonly string _applicationUsersKeyTemplate = "users-{0}-{1}-{2}-{3}";
        private static readonly string _appUsersForAClaimKeyTemplate = "users-{0}-{1}-{2}-{3}";
        private static readonly string _allUsersInARoleKeyTemplate = "users-{0}-{1}-{2}-{3}";


       

        // Roles
        public static string GenerateGetAllApplicationRolesCacheKey(PaginationFilter paginationFilterAppUser)
        {
            return string.Format(_applicationRolesKeyTemplate, paginationFilterAppUser.Search, paginationFilterAppUser.Sort, paginationFilterAppUser.PageNumber, paginationFilterAppUser.PageSize);
        }
        // Users
        public static string GenerateGetAllApplicationUsersCacheKey(PaginationFilter paginationFilterAppUser)
        {
            return string.Format(_applicationUsersKeyTemplate, paginationFilterAppUser.Search, paginationFilterAppUser.Sort, paginationFilterAppUser.PageNumber, paginationFilterAppUser.PageSize);
        }
        // Users for a claim
        public static string GenerateGetAllUsersForAClaimCacheKey(PaginationFilter paginationFilterAppUser)
        {
            return string.Format(_appUsersForAClaimKeyTemplate, paginationFilterAppUser.Search, paginationFilterAppUser.Sort, paginationFilterAppUser.PageNumber, paginationFilterAppUser.PageSize);
        }
        // All Users In A Role
        public static string GenerateGetAllUsersInARoleCacheKey(PaginationFilter paginationFilterAppUser)
        {
            return string.Format(_allUsersInARoleKeyTemplate, paginationFilterAppUser.Search, paginationFilterAppUser.Sort, paginationFilterAppUser.PageNumber, paginationFilterAppUser.PageSize);
        }
      
    }
}
