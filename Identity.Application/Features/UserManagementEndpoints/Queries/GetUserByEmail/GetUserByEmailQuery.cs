using ApplicationSharedKernel.Interfaces;

namespace Identity.Application.Features.UserManagementEndpoints.Queries.GetUserByEmail;

public class GetUserByEmailQuery : ICachedQuery<GetUserByEmailResponse>
{
    public string Email { get; set; }

    public string CacheKey => $"user-by-email-{Email}";

    public TimeSpan? Expiration => null;
}
