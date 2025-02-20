using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Domain.Specifications;

public class GetWalletDomainEntityByEmailSpecification(string email) : BaseSpecification<WalletDomainEntity>(x => x.Email == email)
{
}
