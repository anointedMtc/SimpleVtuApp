using ApplicationSharedKernel.DTO;
using SagaOrchestrationStateMachines.Shared.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetAllSagaInstance;

public sealed class GetAllVtuDataSagaInstanceResponse : ApiBaseResponse
{
    public List<VtuDataSagaOrchestratorInstanceResponseDto>? VtuDataSagaOrchestratorInstanceResponseDto { get; set; }

}
