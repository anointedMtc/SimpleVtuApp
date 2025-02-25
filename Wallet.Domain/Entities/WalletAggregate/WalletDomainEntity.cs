using SharedKernel.Domain;
using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Events;
using Wallet.Domain.Exceptions;

namespace Wallet.Domain.Entities.WalletAggregate;

// Dependent (child) of Owner class... so Owner is a parent... Wallet is the child... 
public class WalletDomainEntity : BaseEntity, IAggregateRoot
{
    //private HashSet<Transfer> _transfers = [];
    private readonly List<Transfer> _transfers = [];

    public Guid WalletDomainEntityId { get; private set; }
    public Guid ApplicationUserId { get; private set; }
    public string Email { get; private set; }

    //public IEnumerable<Transfer> Transfers
    //{
    //    get => _transfers;
    //    set => _transfers = new HashSet<Transfer>(value);
    //}
    public IReadOnlyCollection<Transfer> Transfers => _transfers.AsReadOnly();
    public DateTimeOffset CreatedAt { get; private set; }


    public Guid OwnerId { get; private set; } // Required foreign key property of parent
    public Owner Owner { get; private set; }  // Required reference navigation to principal/parent



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

    public Transfer AddFunds(Amount amount, string reasonWhy) // why are we not stating reason why the funds was added and the date/time it was added???
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

    public Transfer DeductFunds(Amount amount, string reasonWhy) // why are we not stating reason why the funds was added and the date/time it was added???
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


    //private void RaiseWalletAddedDomainEvent(Guid walletId, Guid ownerId)
    //{
    //    var walletAddedDomainEvent = new WalletAddedDomainEvent(this, walletId, OwnerId);

    //    this.AddDomainEvent(walletAddedDomainEvent);
    //}


    private void SecondWayToAddDomainEvent()
    {
        AddDomainEvent(new WalletAddedDomainEvent(this));

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
