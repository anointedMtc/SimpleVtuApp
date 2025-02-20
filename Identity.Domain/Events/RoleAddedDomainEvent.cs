using DomainSharedKernel.Interfaces;

namespace Identity.Domain.Events;

public record RoleAddedDomainEvent(
    string RoleName, 
    string? Description) : IDomainEvent;
