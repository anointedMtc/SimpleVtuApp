using SharedKernel.Domain;
using SharedKernel.Domain.Entities;

namespace Wallet.Domain.Entities.WalletAggregate;

public partial class Transfer : BaseEntity
{
    public Guid TransferId { get; private set; }
    public Amount Amount { get; private set; }
    public TransferDirection Direction { get; private set; }
    public string ReasonWhy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }


    public Guid WalletDomainEntityId { get; private set; }


    //#pragma warning disable CS8618    // Required by Entity Framework
    private Transfer() { }

    public Transfer(Guid walletId, Amount amount,
        TransferDirection direction, string reasonWhy, DateTimeOffset createdAt)
    {
        WalletDomainEntityId = walletId;
        Amount = amount;
        Direction = direction;
        ReasonWhy = reasonWhy;
        CreatedAt = createdAt;
    }

    public static Transfer Incoming(Guid walletId, Amount amount,
        string reasonWhy, DateTimeOffset createdAt)
    {
        return new Transfer(
            walletId,
            amount,
            TransferDirection.In,
            reasonWhy,
            createdAt
        );
    }

    public static Transfer Outgoing(Guid walletId, Amount amount,
        string reasonWhy, DateTimeOffset createdAt)
    {
        return new Transfer(
            walletId,
            amount,
            TransferDirection.Out,
            reasonWhy,
            createdAt
        );
    }

    public override string ToString()
    {
        return Amount.ToString();
    }
}
