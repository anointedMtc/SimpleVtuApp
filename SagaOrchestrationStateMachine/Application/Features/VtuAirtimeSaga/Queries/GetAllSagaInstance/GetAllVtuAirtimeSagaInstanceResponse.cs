using ApplicationSharedKernel.DTO;
using SagaOrchestrationStateMachines.Shared.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetAllSagaInstance;

public sealed class GetAllVtuAirtimeSagaInstanceResponse : ApiBaseResponse
{
    public List<VtuAirtimeSagaOrchestratorInstanceResponseDto>? VtuAirtimeSagaOrchestratorInstanceResponseDto { get; set; }

}
