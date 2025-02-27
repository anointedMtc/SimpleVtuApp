using SharedKernel.Domain;
using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Events;
using Wallet.Domain.Exceptions;

namespace Wallet.Domain.Entities.WalletAggregate;

public class WalletDomainEntity : BaseEntity, IAggregateRoot
{
    private readonly List<Transfer> _transfers = [];

    public Guid WalletDomainEntityId { get; private set; }
    public Guid ApplicationUserId { get; private set; }
    public string Email { get; private set; }
    public IReadOnlyCollection<Transfer> Transfers => _transfers.AsReadOnly();
    public DateTimeOffset CreatedAt { get; private set; }


    public Guid OwnerId { get; private set; } 
    public Owner Owner { get; private set; }  



    //#pragma warning disable CS8618    // Required by Entity Framework
    private WalletDomainEntity() { }

    public WalletDomainEntity(Guid ownerId, Guid applicationUserId, string email)
    {
        OwnerId = ownerId;
        ApplicationUserId = applicationUserId;
        Email = email;
        CreatedAt = DateTimeOffset.UtcNow;

        // raise the domain event
        RaiseWalletAddedDomainEvent();
    }


    public static WalletDomainEntity Create(Guid ownerId, Guid applicationUserId, string email)
    {

        return new WalletDomainEntity(ownerId, applicationUserId, email);
    }


    public IReadOnlyCollection<Transfer> TransferFunds(WalletDomainEntity receiver, Amount amount, string reasonWhy)
    {

        var outTransfer = DeductFunds(amount, reasonWhy);
        var inTransfer = receiver.AddFunds(amount, reasonWhy);

        AddDomainEvent(new FundsTransferredDomainEvent(WalletDomainEntityId, receiver.WalletDomainEntityId, amount));

        return [outTransfer, inTransfer];
    }

    public Transfer AddFunds(Amount amount, string reasonWhy) 
    {
        if (amount <= 0)
        {
            throw new InvalidTransferAmountException(amount);
        }
        var createdAt = DateTimeOffset.UtcNow;
        var transfer = Transfer.Incoming(WalletDomainEntityId, amount, reasonWhy, createdAt);
        _transfers.Add(transfer);

        RaiseFundsAddedDomainEvent(amount);

        return transfer;
    }

    public Transfer DeductFunds(Amount amount, string reasonWhy) 
    {
        if (amount <= 0)
        {
            throw new InvalidTransferAmountException(amount);
        }

        if (CurrentAmount() < amount)
        {
            throw new InsufficientWalletFundsException(WalletDomainEntityId);
        }

        var createdAt = DateTimeOffset.UtcNow;
        var transfer = Transfer.Outgoing(WalletDomainEntityId, amount, reasonWhy, createdAt);
        _transfers.Add(transfer);

        return transfer;
    }

    public Amount CurrentAmount() => SumIncomingAmount() - SumOutgoingAmount();

    private Amount SumIncomingAmount()
    {
        return _transfers.Where(x => x.Direction == Transfer.TransferDirection.In).Sum(x => x.Amount);
    }

    private Amount SumOutgoingAmount()
    {
        return _transfers.Where(x => x.Direction == Transfer.TransferDirection.Out).Sum(x => x.Amount);
    }


    private void RaiseWalletAddedDomainEvent()
    {
        var walletAddedDomainEvent = new WalletAddedDomainEvent(this);

        AddDomainEvent(walletAddedDomainEvent);
    }


    private void RaiseFundsAddedDomainEvent(Amount amount)
    {
        AddDomainEvent(new FundsAddedDomainEvent(WalletDomainEntityId, OwnerId, amount));
    }


    public override string ToString()
    {
        return WalletDomainEntityId.ToString();
    }
}
