using ApplicationSharedKernel.DTO;
using SagaOrchestrationStateMachines.Shared.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetAllSagaInstance;

public sealed class GetAllUserCreatedSagaInstanceResponse : ApiBaseResponse
{
    public List<UserCreatedSagOrchestratorInstanceResponseDto>? UserCreatedSagOrchestratorInstanceResponseDto { get; set; }

}
