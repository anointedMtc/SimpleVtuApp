using SharedKernel.Domain.Interfaces;

namespace Wallet.Domain.Events;

// we only need this event to create wallet for the owner and that's all... but maybe we may decide to send email as well
public record OwnerAddedDomainEvent(
    Guid OwnerId,
    Guid ApplicationUser,
    string OwnerEmail) : IDomainEvent;
