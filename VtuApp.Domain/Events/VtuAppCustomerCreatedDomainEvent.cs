using SharedKernel.Domain.Interfaces;

namespace VtuApp.Domain.Events;

public record VtuAppCustomerCreatedDomainEvent(
    Guid ApplicationUserId,
    string Email
    ) : IDomainEvent;
