using DomainSharedKernel.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Domain.Events;

public record StarAchievedByCustomerDomainEvent(
    Customer Customer,
    DateTimeOffset CreatedAt,
    decimal DiscountGiven) : IDomainEvent;


//{
//    public Customer Customer { get; set; }
//    public DateTimeOffset CreatedAt { get; set; }
//}
