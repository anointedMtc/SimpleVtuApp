using MediatR;
using SharedKernel.Application.Interfaces;
using Wallet.Application.HelperClasses;

namespace Wallet.Application.Features.Queries.GetOwnerAndWalletByEmail;

public sealed class GetOwnerAndWalletByEmailQuery : ICachedQuery<GetOwnerAndWalletByEmailResponse>
{
    public string Email { get; set; }

    public string CacheKey => CacheHelperWallet.GenerateGetOwnerAndWalletByEmailCacheKey(Email);

    public TimeSpan? Expiration => null;
}
