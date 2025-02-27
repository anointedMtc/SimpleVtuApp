using SharedKernel.Domain.Interfaces;

namespace Wallet.Domain.Events;

public record FundsTransferredDomainEvent(
    Guid FromWalletId, 
    Guid ToWalletId, 
    decimal Amount) : IDomainEvent;