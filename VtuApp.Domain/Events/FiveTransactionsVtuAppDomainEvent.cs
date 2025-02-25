using SharedKernel.Domain.Interfaces;

namespace VtuApp.Domain.Events;

public record FiveTransactionsVtuAppDomainEvent(
    Guid CustomerId,
    DateTimeOffset CreatedAt) : IDomainEvent;