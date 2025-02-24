using ApplicationSharedKernel.DTO;
using SagaOrchestrationStateMachines.Shared.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetSingleInstance;

public sealed class GetVtuAirtimeOrderedSagaStateInstanceResponse
    : ApiBaseResponse
{
    public VtuAirtimeSagaOrchestratorInstanceResponseDto? VtuAirtimeSagaOrchestratorInstanceResponseDto { get; set; }
}
