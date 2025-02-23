using DomainSharedKernel;
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


    // there is an Id of the Principal/parent class on the dependent/child class which would be used as foreign key / navigation
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










//public class TypeOfTransaction
//{
//    public const string Data = "data";
//    public const string Airtime = "airtime";
//}

//public class NetWorkProvider
//{
//    public const string Mtn = "Mtn";
//    public const string Airtel = "Airtel";
//    public const string Glo = "Glo";
//    public const string NineMobile = "9Mobile";
//}

