﻿using Identity.Application.HelperClasses;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserByEmail;

public class GetUserByEmailQuery : ICachedQuery<GetUserByEmailResponse>
{
    public string Email { get; set; }

    public string CacheKey => CacheHelpers.GenerateGetUserByEmailQueryCacheKey(Email);

    public TimeSpan? Expiration => null;
}
