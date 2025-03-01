using SharedKernel.Common.Constants;
using SharedKernel.Domain;

namespace VtuApp.Domain.Entities.VtuModelAggregate;

public class VtuAppTransfer : BaseEntity
{
    public Guid Id { get; private set; }
    public VtuAmount AmountTransfered { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public VtuAmount InitialBalance { get; private set; }
    public VtuAmount FinalBalance { get; private set; }
    public TransferDirection TransferDirection { get; private set; }
    public string ReasonWhy { get; private set; }


    public Guid CustomerId { get; private set; }


    public VtuAppTransfer(VtuAmount amountTransfered, DateTimeOffset createdAt, 
        VtuAmount initialBalance, VtuAmount finalBalance, 
        TransferDirection transferDirection, string reasonWhy, Guid customerId)
    {
        AmountTransfered = amountTransfered;
        CreatedAt = createdAt;
        InitialBalance = initialBalance;
        FinalBalance = finalBalance;
        TransferDirection = transferDirection;
        ReasonWhy = reasonWhy;
        CustomerId = customerId;
    }


    //#pragma warning disable CS8618    // Required by Entity Framework
    public VtuAppTransfer() { }


}
