using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.HelperClasses
{
    public static class CacheHelpers
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(30);
        private static readonly string _applicationRolesKeyTemplate = "Roles-all-{0}-{1}-{2}-{3}";
        private static readonly string _applicationUsersKeyTemplate = "Users-all-{0}-{1}-{2}-{3}";
        private static readonly string _appUsersForAClaimKeyTemplate = "Users-for-a-Claim-{0}-{1}-{2}-{3}";
        private static readonly string _allUsersInARoleKeyTemplate = "Users-in-a-Role-{0}-{1}-{2}-{3}";
        
        private static readonly string _getMyDetailsQueryKeyTemplate = "User-by-Id-{0}-{1}-{2}-{3}";
        private static readonly string _getRoleByIdQueryKeyTemplate = "Role-by-Id-{0}-{1}-{2}-{3}";
        private static readonly string _getRoleByNameQueryKeyTemplate = "Role-by-Name-{0}-{1}-{2}-{3}";
        private static readonly string _getUserByEmailQueryKeyTemplate = "User-by-Email-{0}-{1}-{2}-{3}";
        private static readonly string _getUserByIdQueryKeyTemplate = "User-by-Id-{0}-{1}-{2}-{3}";
        private static readonly string _getUserClaimsQueryKeyTemplate = "Claims-for-User-With-Id-{0}-{1}-{2}-{3}";


       

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



        public static string GenerateGetMyDetailsQueryCacheKey(string id)
        {
            return string.Format(_getMyDetailsQueryKeyTemplate, " ", " ", " ", id);
        }

        public static string GenerateGetRoleByIdQueryCacheKey(Guid id)
        {
            return string.Format(_getRoleByIdQueryKeyTemplate, " ", " ", " ", id);
        }

        public static string GenerateGetRoleByNameQueryCacheKey(string name)
        {
            return string.Format(_getRoleByNameQueryKeyTemplate, " ", " ", " ", name);
        }

        public static string GenerateGetUserByEmailQueryCacheKey(string email)
        {
            return string.Format(_getUserByEmailQueryKeyTemplate, " ", " ", " ", email);
        }

        public static string GenerateGetUserByIdQueryCacheKey(Guid id)
        {
            return string.Format(_getUserByIdQueryKeyTemplate, " ", " ", " ", id);
        }

        public static string GenerateGetUserClaimsQueryCacheKey(Guid id)
        {
            return string.Format(_getUserClaimsQueryKeyTemplate, " ", " ", " ", id);
        }


    }
}
