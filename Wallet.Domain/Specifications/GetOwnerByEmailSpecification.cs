using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities;

namespace Wallet.Domain.Specifications;

public sealed class GetOwnerByEmailSpecification(string email) : BaseSpecification<Owner>(x => x.Email == email)
{

}
