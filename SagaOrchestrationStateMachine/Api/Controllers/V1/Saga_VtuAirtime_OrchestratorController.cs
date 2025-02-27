using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetAllSagaInstance;
using SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetSingleInstance;
using SharedKernel.Api.Controllers;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class Saga_VtuAirtime_OrchestratorController : ApiBaseController
{
    [HttpGet("get-vtu-airtime-saga-instance/{correlationId}")]
    public async Task<ActionResult<GetVtuAirtimeOrderedSagaStateInstanceResponse>> GetVtuAirtimeSagaInstance(Guid correlationId)
    {
        var result = await Mediator.Send(new GetVtuAirtimeOrderedSagaStateInstanceQuery() { CorrelationId = correlationId });

        return Ok(result);
    }


    [HttpGet("get-all-vtuAirtime-saga-instance")]
    public async Task<ActionResult<Pagination<GetAllVtuAirtimeSagaInstanceResponse>>> GetAllUserCreatedSagaInstance([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await Mediator.Send(new GetAllVtuAirtimeSagaInstanceQuery(paginationFilter));

        var endpointUrl = $"{Request.Scheme}://{Request.Host}{Request.Path.Value}";

        return new Pagination<GetAllVtuAirtimeSagaInstanceResponse>(paginationFilter, result.TotalRecords, result.Data, endpointUrl);
    }

}
