using VtuApp.Shared.Constants;

namespace SagaOrchestrationStateMachines.Shared.IntegrationEvents.VtuAirtimeSaga;

public record PrepareForSecondRetryVtuAirtimeOrderEvent
{
    public Guid ApplicationUserId { get; init; }
    public string Email { get; init; }
    public Guid VtuTransactionId { get; init; }
    public NetworkProvider NetworkProvider { get; init; }
    public decimal AmountToPurchase { get; init; }
    public string Reciever { get; init; }
}
