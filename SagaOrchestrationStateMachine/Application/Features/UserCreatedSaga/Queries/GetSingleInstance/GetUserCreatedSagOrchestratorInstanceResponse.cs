using ApplicationSharedKernel.DTO;
using SagaOrchestrationStateMachines.Shared.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetSingleInstance;

public sealed class GetUserCreatedSagOrchestratorInstanceResponse
    : ApiBaseResponse
{
    public UserCreatedSagOrchestratorInstanceResponseDto? UserCreatedSagOrchestratorInstanceResponseDto { get; set; }
}
