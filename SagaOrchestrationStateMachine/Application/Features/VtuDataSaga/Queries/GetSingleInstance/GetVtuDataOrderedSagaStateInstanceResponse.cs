using ApplicationSharedKernel.DTO;
using SagaOrchestrationStateMachines.Shared.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetSingleInstance;

public sealed class GetVtuDataOrderedSagaStateInstanceResponse
    : ApiBaseResponse
{
    public VtuDataSagaOrchestratorInstanceResponseDto? VtuDataSagaOrchestratorInstanceResponseDto { get; set; }
}
