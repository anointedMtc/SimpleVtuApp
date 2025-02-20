﻿namespace SagaOrchestrationStateMachines.Common.DTO;

public class UserCreatedSagOrchestratorInstanceResponseDto
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public Guid ApplicationUserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public decimal RegisterationBonus { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public int UserCreatedInAllModulesEventStatus { get; set; }

    public Guid? NotifyApplicationUserScheduleEventTokenId { get; set; }
}
