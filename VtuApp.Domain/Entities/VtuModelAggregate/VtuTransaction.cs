using SharedKernel.Domain;
using VtuApp.Shared.Constants;

namespace VtuApp.Domain.Entities.VtuModelAggregate;

public class VtuTransaction : BaseEntity
{
    public Guid Id { get; private set; }
    public TypeOfTransaction TypeOfTransaction { get; private set; }
    public NetworkProvider NetWorkProvider { get; private set; }
    public VtuAmount Amount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Status Status { get; private set; }
    public VtuAmount Discount { get; private set; }


    public Guid CustomerId { get; private set; }


    //#pragma warning disable CS8618    // Required by Entity Framework
    private VtuTransaction() { }

    public VtuTransaction(TypeOfTransaction typeOfTransaction,
        NetworkProvider netWorkProvider, VtuAmount vtuAmount,
        DateTimeOffset createdAt, Status status, Guid customerId, VtuAmount discount)
    {
        TypeOfTransaction = typeOfTransaction;
        NetWorkProvider = netWorkProvider;
        Amount = vtuAmount;
        CreatedAt = createdAt;
        Status = status;
        CustomerId = customerId;
        Discount = discount;
    }


    public void UpdateStatus(Status status)
    {
        Status = status; 
    }

    public void AddDiscount(VtuAmount amount)
    {
        Amount = amount;
    }


}

