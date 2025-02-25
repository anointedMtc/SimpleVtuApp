using MediatR;
using SharedKernel.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedKernel.Domain;

public abstract class BaseEntity
{
    // we are not specifying any Id here because we want to have the liberty to choose between int, long or Guid per entity as we see fit

    // We are also not doing any equal/comparison operator because the Id-Equality is enough for us... we are never going to have two Product or Order entity with the Id of 11... our database will make sure they are Unique an Id equality is enough for entities

    private readonly List<IDomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
