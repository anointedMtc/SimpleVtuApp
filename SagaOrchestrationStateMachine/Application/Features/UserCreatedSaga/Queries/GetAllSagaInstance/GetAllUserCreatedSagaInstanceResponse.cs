using SagaOrchestrationStateMachines.Shared.DTO;
using SharedKernel.Application.DTO;

namespace SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetAllSagaInstance;

public sealed class GetAllUserCreatedSagaInstanceResponse : ApiBaseResponse
{
    public List<UserCreatedSagOrchestratorInstanceResponseDto>? UserCreatedSagOrchestratorInstanceResponseDto { get; set; }

}
