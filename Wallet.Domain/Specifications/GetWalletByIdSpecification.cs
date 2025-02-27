using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Domain.Specifications;

public class GetWalletByIdSpecification : BaseSpecification<WalletDomainEntity>
{
    public GetWalletByIdSpecification(Guid walletId) : base(x => x.WalletDomainEntityId == walletId)
    {

    }
}
