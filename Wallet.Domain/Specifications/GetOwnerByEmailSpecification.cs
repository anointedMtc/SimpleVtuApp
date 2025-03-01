using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities;

namespace Wallet.Domain.Specifications;

public sealed class GetOwnerByEmailSpecification : BaseSpecification<Owner>
{
    public GetOwnerByEmailSpecification(string email) : base(x => x.Email.ToLower() == email.ToLower())
    {
        //AddInclude(x => x.WalletDomainEntity);
    }
}
