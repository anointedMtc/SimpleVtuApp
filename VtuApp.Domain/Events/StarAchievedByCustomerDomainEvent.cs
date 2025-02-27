using SharedKernel.Domain.Interfaces;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Domain.Events;

public record StarAchievedByCustomerDomainEvent(
    Customer Customer,
    DateTimeOffset CreatedAt,
    decimal DiscountGiven) : IDomainEvent;

