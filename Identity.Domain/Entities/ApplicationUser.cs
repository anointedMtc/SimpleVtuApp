using Microsoft.AspNetCore.Identity;
using SharedKernel.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Domain.Entities;

public class ApplicationUser : IdentityUser, IAggregateRoot
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";    
    public string ConstUserName { get; set; }
    public string Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset LastLogin { get; set; } 
    public string? RefreshToken { get; set; } 
    public DateTimeOffset? RefreshTokenExpiryTime { get; set; }   
    public int RateLimit { get; set; }





    // Cannot have multiple base classes... So...

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
