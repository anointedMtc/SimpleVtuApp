namespace SagaOrchestrationStateMachines.Common.Entities;

public class SagaStateMachineEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool ManuallyCompleted { get; set; }
    public string CompletedBy { get; set; }
}
