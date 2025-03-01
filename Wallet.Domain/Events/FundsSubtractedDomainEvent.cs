using SharedKernel.Domain.Interfaces;

namespace Wallet.Domain.Events;

public record FundsSubtractedDomainEvent(
    Guid WalletId,
    Guid OwnerId,
    Guid ApplicationUserId,
    string Email,
    //string FirstName,
    string ReasonWhy,
    Guid TransferId,
    decimal Amount,
    decimal FinalBalance,
    DateTimeOffset CreatedAt) : IDomainEvent;
