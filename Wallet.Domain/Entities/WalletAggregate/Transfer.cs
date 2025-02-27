using SharedKernel.Domain;
using SharedKernel.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Wallet.Domain.Entities.WalletAggregate;

public class Transfer : BaseEntity
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


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransferDirection
    {
        In,
        Out
    }

    public override string ToString()
    {
        return Amount.ToString();
    }
}
