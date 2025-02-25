using SagaOrchestrationStateMachines.Shared.DTO;
using SharedKernel.Application.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetAllSagaInstance;

public sealed class GetAllVtuDataSagaInstanceResponse : ApiBaseResponse
{
    public List<VtuDataSagaOrchestratorInstanceResponseDto>? VtuDataSagaOrchestratorInstanceResponseDto { get; set; }

}
