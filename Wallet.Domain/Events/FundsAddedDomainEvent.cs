using SharedKernel.Domain.Interfaces;

namespace Wallet.Domain.Events;

public record FundsAddedDomainEvent(
    Guid WalletId, 
    Guid OwnerId, 
    decimal Amount) : IDomainEvent;
