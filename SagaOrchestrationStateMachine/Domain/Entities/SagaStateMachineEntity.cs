namespace SagaOrchestrationStateMachines.Domain.Entities;

// just to show that we can still persits other types of data if we like...
public class SagaStateMachineEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool ManuallyCompleted { get; set; }
    public string CompletedBy { get; set; }
}
