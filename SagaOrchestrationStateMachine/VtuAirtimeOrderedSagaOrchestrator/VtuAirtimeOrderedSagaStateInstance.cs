using DomainSharedKernel.Interfaces;
using MassTransit;
using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.VtuAirtimeOrderedSagaOrchestrator;

public sealed class VtuAirtimeOrderedSagaStateInstance : SagaStateMachineInstance, IAggregateRoot
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public byte[] RowVersion { get; set; }



    public Guid ApplicationUserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid VtuTransactionId { get; set; }
    public NetworkProvider NetworkProvider { get; set; }  
    public decimal AmountToPurchase { get; set; }  // mtn 200 is what the customer wants to buy and the value he/she would actually recieve
    public decimal PricePaid { get; set; }          // 180 is what we charged the customer because of discount... but he will recieve 200 airtime

    public string Receiver { get; set; }
    public string Sender { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal FinalBalance { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    //public decimal Discount { get; set; }
    //public TypeOfTransaction TypeOfTransaction { get; set; }


    public Guid? SecondRetryVtuAirtimeScheduleEventTokenId { get; set; }

}
