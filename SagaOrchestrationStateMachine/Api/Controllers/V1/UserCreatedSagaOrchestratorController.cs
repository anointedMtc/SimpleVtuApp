using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetAllSagaInstance;
using SagaOrchestrationStateMachines.Application.Features.UserCreatedSaga.Queries.GetSingleInstance;
using SharedKernel.Api.Controllers;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class UserCreatedSagaOrchestratorController : ApiBaseController
{
    [HttpGet("get-single-user-created-saga-instance/{correlationId}")]
    public async Task<ActionResult<GetUserCreatedSagOrchestratorInstanceResponse>> GetUserCreatedSagaInstance(Guid correlationId)
    {
        var result = await Mediator.Send(new GetUserCreatedSagOrchestratorInstanceQuery() { CorrelationId = correlationId });

        return Ok(result);
    }


    [HttpGet("get-all-user-created-saga-instance")]
    public async Task<ActionResult<Pagination<GetAllUserCreatedSagaInstanceResponse>>> GetAllUserCreatedSagaInstance([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await Mediator.Send(new GetAllUserCreatedSagaInstanceQuery(paginationFilter));

        return Ok(result);
    }
}
