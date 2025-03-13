using SharedKernel.Domain;
using SharedKernel.Domain.Interfaces;
using Wallet.Domain.Events;
using Wallet.Domain.Exceptions;

namespace Wallet.Domain.Entities.WalletAggregate;

public class WalletDomainEntity : BaseEntity, IAggregateRoot
{
    private readonly List<Transfer> _transfers = [];
    //private readonly Amount _walletBalance;
     
    public Guid WalletDomainEntityId { get; private set; }
    public Guid ApplicationUserId { get; private set; }
    public string Email { get; private set; }
    public IReadOnlyCollection<Transfer> Transfers => _transfers.AsReadOnly();
    public DateTimeOffset CreatedAt { get; private set; }


    public Guid OwnerId { get; private set; }
    public Owner Owner { get; private set; }

    public Amount WalletBalance { get; private set; }

    //public Amount WalletBalance
    //{
    //    get => _walletBalance;
    //    set => CurrentAmount();
    //}


    //#pragma warning disable CS8618    // Required by Entity Framework
    private WalletDomainEntity() { }

    public WalletDomainEntity(Guid ownerId, Guid applicationUserId, string email)
    {
        OwnerId = ownerId;
        ApplicationUserId = applicationUserId;
        Email = email;
        CreatedAt = DateTimeOffset.UtcNow;
        WalletBalance = 0M;

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

        AddDomainEvent(new FundsTransferredDomainEvent(
            //Sender's Details
            WalletDomainEntityId,
            outTransfer.TransferId,
            Owner.FirstName,
            Email,
            WalletBalance,
            // Receiver's Details
            receiver.WalletDomainEntityId,
            inTransfer.TransferId,
            receiver.Owner.FirstName,
            receiver.Owner.Email,
            receiver.WalletBalance,
            // common
            amount,
            DateTimeOffset.UtcNow));

        return [outTransfer, inTransfer];
    }

    public Transfer AddFunds(Amount amount, string reasonWhy) 
    {
        if (amount <= 0)
        {
            throw new InvalidTransferAmountException(amount);
        }
        var createdAt = DateTimeOffset.UtcNow;
        var referenceId = Guid.NewGuid();
        var transfer = Transfer.Incoming(WalletDomainEntityId, amount, reasonWhy, createdAt, referenceId);
        _transfers.Add(transfer);

        WalletBalance += amount;

        RaiseFundsAddedDomainEvent(amount, reasonWhy, referenceId, WalletBalance);

        return transfer;
    }

    public Transfer DeductFunds(Amount amount, string reasonWhy) 
    {
        if (amount <= 0)
        {
            throw new InvalidTransferAmountException(amount);
        }

        if (WalletBalance < amount)
        {
            throw new InsufficientWalletFundsException(WalletDomainEntityId);
        }

        var createdAt = DateTimeOffset.UtcNow;
        var referenceId = Guid.NewGuid();
        var transfer = Transfer.Outgoing(WalletDomainEntityId, amount, reasonWhy, createdAt, referenceId);
        _transfers.Add(transfer);

        WalletBalance -= amount;

        RaiseFundsSubtractedDomainEvent(amount, reasonWhy, referenceId, WalletBalance);

        return transfer;
    }

    private void RaiseWalletAddedDomainEvent()
    {
        var walletAddedDomainEvent = new WalletAddedDomainEvent(this);

        AddDomainEvent(walletAddedDomainEvent);
    }

    private void RaiseFundsAddedDomainEvent(Amount amount, string reasonWhy, Guid referenceId, decimal finalBalance)
    {
        AddDomainEvent(new FundsAddedDomainEvent(
            WalletDomainEntityId, 
            OwnerId, 
            ApplicationUserId,
            Email,
            //Owner.FirstName,
            reasonWhy,
            referenceId,
            amount,
            finalBalance,
            DateTimeOffset.UtcNow));
    }

    private void RaiseFundsSubtractedDomainEvent(Amount amount, string reasonWhy, Guid referenceId, decimal finalBalance)
    {
        AddDomainEvent(new FundsSubtractedDomainEvent(
            WalletDomainEntityId,
            OwnerId,
            ApplicationUserId,
            Email,
            //Owner.FirstName,
            reasonWhy,
            referenceId,
            amount,
            finalBalance,
            DateTimeOffset.UtcNow));
    }

    public override string ToString()
    {
        return $"WalletId:{Email} ---- WalletBalance:{WalletBalance}";
    }
}
