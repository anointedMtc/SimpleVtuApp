using DomainSharedKernel;
using System.Text.Json.Serialization;

namespace Wallet.Domain.Entities.WalletAggregate;

public class Transfer : BaseEntity
{
    public Guid TransferId { get; private set; }
    public Guid ReferenceId { get; private set; }
    public Guid WalletId { get; private set; }
    public Amount Amount { get; private set; }
    public TransferDirection Direction { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }


    //#pragma warning disable CS8618    // Required by Entity Framework
    private Transfer() { }

    // what you must use to 
    public Transfer(Guid walletId, Amount amount,
        TransferDirection direction, DateTimeOffset createdAt, Guid referenceId)
    {
        WalletId = walletId;
        Amount = amount;
        Direction = direction;
        CreatedAt = createdAt;
        ReferenceId = referenceId;
    }

    public static Transfer Incoming(Guid walletId, Amount amount,
        DateTimeOffset createdAt, Guid referenceId)
    {
        return new Transfer(
            walletId,
            amount,
            TransferDirection.In,
            createdAt,
            referenceId
        );
    }

    public static Transfer Outgoing(Guid walletId, Amount amount,
        DateTimeOffset createdAt, Guid referenceId)
    {
        return new Transfer(
            walletId,
            amount,
            TransferDirection.Out,
            createdAt,
            referenceId
        );
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransferDirection
    {
        In,
        Out
    }



    public override string ToString()
    {
        return Amount.ToString();
        //return TransferId.ToString();
    }
}
