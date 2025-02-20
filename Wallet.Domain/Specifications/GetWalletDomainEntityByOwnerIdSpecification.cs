using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Domain.Specifications;

public class GetWalletDomainEntityByOwnerIdSpecification(Guid ownerId) : BaseSpecification<WalletDomainEntity>(x => x.OwnerId == ownerId)
{

}
