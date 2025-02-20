using DomainSharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Domain.Entities;

public class ApplicationRole : IdentityRole, IAggregateRoot
{
    public string? Description { get; set; }



    //public ApplicationRole(string roleName, string? description)
    //{
    //    Name = roleName;
    //    Description = description;

    //    AddDomainEvent(new RoleAddedDomainEvent(roleName, description));
    //}


   





    // Cannot have multiple base classes... So..

    private readonly List<IDomainEvent> _domainEvents = [];

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
