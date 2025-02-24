using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.DTO;

public class VtuAirtimeSagaOrchestratorInstanceResponseDto
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public Guid ApplicationUserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid VtuTransactionId { get; set; }
    public NetworkProvider NetworkProvider { get; set; }
    public decimal AmountToPurchase { get; set; }
    public decimal PricePaid { get; set; }

    public string Receiver { get; set; }
    public string Sender { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal FinalBalance { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Guid? SecondRetryVtuAirtimeScheduleEventTokenId { get; set; }

}
