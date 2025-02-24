using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetSingleInstance;
using SharedKernel.Api.Controllers;

namespace SagaOrchestrationStateMachines.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public sealed class VtuDataSagaOrchestratorController : ApiBaseController
{
    [HttpGet("get-vtu-data-saga-instance/{correlationId}")]
    public async Task<ActionResult<GetVtuDataOrderedSagaStateInstanceResponse>> GetVtuDataSagaInstance(Guid correlationId)
    {
        var result = await Mediator.Send(new GetVtuDataOrderedSagaStateInstanceQuery() { CorrelationId = correlationId });

        return Ok(result);
    }
}
