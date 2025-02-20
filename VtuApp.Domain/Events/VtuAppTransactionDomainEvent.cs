using DomainSharedKernel.Interfaces;

namespace VtuApp.Domain.Events;

public record VtuAppTransactionDomainEvent(
    Guid CustomerId,
    string TypeOfTransaction,
    string NetWorkProvider,
    decimal Amount,
    DateTimeOffset CreatedAt,
    string Status) : IDomainEvent;



