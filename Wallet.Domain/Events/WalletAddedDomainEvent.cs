using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Domain.Events;

public record WalletAddedDomainEvent(
    WalletDomainEntity Wallet) : IDomainEvent;
