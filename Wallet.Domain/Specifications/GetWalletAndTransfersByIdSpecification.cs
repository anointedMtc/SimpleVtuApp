using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Domain.Specifications;

public sealed class GetWalletAndTransfersByIdSpecification : BaseSpecification<WalletDomainEntity>
{
    public GetWalletAndTransfersByIdSpecification(Guid id) : base(x => x.WalletDomainEntityId == id)
    {
        AddInclude(x => x.Transfers);
    }
}
