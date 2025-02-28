using SharedKernel.Domain.Interfaces;

namespace Wallet.Domain.Events;

public record FundsSubtractedDomainEvent(
    Guid WalletId,
    Guid OwnerId,
    decimal Amount) : IDomainEvent;
