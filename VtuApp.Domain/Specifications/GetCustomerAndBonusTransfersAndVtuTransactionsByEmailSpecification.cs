using SharedKernel.Domain.HelperClasses;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Domain.Specifications;

public sealed class GetCustomerAndBonusTransfersAndVtuTransactionsByEmailSpecification
    : BaseSpecification<Customer>
{
    public GetCustomerAndBonusTransfersAndVtuTransactionsByEmailSpecification(string email)
        : base(x => x.Email.ToLower() == email.ToLower())
    {
        AddInclude(x => x.VtuBonusTransfers);
        AddInclude(x => x.VtuTransactions);
    }
}
