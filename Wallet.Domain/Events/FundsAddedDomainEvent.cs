using SharedKernel.Domain.Interfaces;

namespace Wallet.Domain.Events;

public record FundsAddedDomainEvent(
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
