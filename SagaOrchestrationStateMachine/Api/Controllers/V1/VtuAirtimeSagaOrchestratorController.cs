using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetSingleInstance;
using SharedKernel.Api.Controllers;

namespace SagaOrchestrationStateMachines.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class VtuAirtimeSagaOrchestratorController : ApiBaseController
{
    [HttpGet("get-vtu-airtime-saga-instance/{correlationId}")]
    public async Task<ActionResult<GetVtuAirtimeOrderedSagaStateInstanceResponse>> GetVtuAirtimeSagaInstance(Guid correlationId)
    {
        var result = await Mediator.Send(new GetVtuAirtimeOrderedSagaStateInstanceQuery() { CorrelationId = correlationId });

        return Ok(result);
    }
}
