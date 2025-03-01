using SharedKernel.Domain.Interfaces;

namespace VtuApp.Domain.Events;

public record FiveTransactionsVtuAppDomainEvent(
    Guid CustomerId,
    string Email,
    string FirstName,
    decimal BonusForFiveTransactions,
    decimal FinalVtuBonusBalance,
    DateTimeOffset CreatedAt) : IDomainEvent;


