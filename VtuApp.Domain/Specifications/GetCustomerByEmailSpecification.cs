using SharedKernel.Domain.HelperClasses;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Domain.Specifications;

public sealed class GetCustomerByEmailSpecification : BaseSpecification<Customer>
{
    public GetCustomerByEmailSpecification(string email) : base(x => x.Email.ToLower() == email.ToLower())
    {
        
    }
}
