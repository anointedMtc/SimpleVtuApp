﻿using SharedKernel.Domain;
using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Entities.WalletAggregate;
using Wallet.Domain.Events;

namespace Wallet.Domain.Entities;

// Principal (parent) of walletDomainEntity class... 
public class Owner : BaseEntity, IAggregateRoot
{
    public Guid OwnerId { get; private set; }
    public Guid ApplicationUserId { get; private set; }  // this is going to be the same with the userId generated by the Users/Auth Module...
    public string Email { get; private set; }   // Email of the user
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }


    // Reference navigation to dependent
    // A good way to remember this is that foreign key is placed inside the child/dependent
    public WalletDomainEntity WalletDomainEntity { get; private set; }


    //#pragma warning disable CS8618    // Required by Entity Framework
    private Owner() { }


    public Owner(Guid applicationUserId, string userEmail, string userFirstName, string userLastName)
    {
        ApplicationUserId = applicationUserId;
        Email = userEmail ?? throw new ArgumentNullException(nameof(userEmail));
        FirstName = userFirstName ?? throw new ArgumentNullException(nameof(userFirstName));
        LastName = userLastName ?? throw new ArgumentNullException(nameof(userLastName));
        CreatedAt = DateTimeOffset.UtcNow;

        // we would call the add Wallet manually so that it can properly pass down the OwnerId... the event was consumed so fast it wasn't populating the ownerId before trying to create the wallet
        //AddDomainEvent(new OwnerAddedDomainEvent(OwnerId, ApplicationUserId, Email));
    }


    public static Owner Create(Guid userId, string userEmail, string userFirstName, string userLastName)
    {
        return new Owner(userId, userEmail, userFirstName, userLastName);
    }



    public WalletDomainEntity CreateWalletForThisOwner()
    {
        return new WalletDomainEntity(OwnerId, ApplicationUserId, Email);
    }






    public override string ToString()
    {
        return Email;
    }










}
