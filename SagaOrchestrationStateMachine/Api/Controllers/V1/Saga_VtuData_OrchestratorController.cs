using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SagaOrchestrationStateMachines.Application.Features.VtuAirtimeSaga.Queries.GetAllSagaInstance;
using SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetAllSagaInstance;
using SagaOrchestrationStateMachines.Application.Features.VtuDataSaga.Queries.GetSingleInstance;
using SharedKernel.Api.Controllers;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public sealed class Saga_VtuData_OrchestratorController : ApiBaseController
{
    [HttpGet("get-vtu-data-saga-instance/{correlationId}")]
    public async Task<ActionResult<GetVtuDataOrderedSagaStateInstanceResponse>> GetVtuDataSagaInstance(Guid correlationId)
    {
        var result = await Mediator.Send(new GetVtuDataOrderedSagaStateInstanceQuery() { CorrelationId = correlationId });

        return Ok(result);
    }

    [HttpGet("get-all-vtuData-saga-instance")]
    public async Task<ActionResult<Pagination<GetAllVtuDataSagaInstanceResponse>>> GetAllUserCreatedSagaInstance([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await Mediator.Send(new GetAllVtuDataSagaInstanceQuery(paginationFilter));

        var endpointUrl = $"{Request.Scheme}://{Request.Host}{Request.Path.Value}";

        return new Pagination<GetAllVtuDataSagaInstanceResponse>(paginationFilter, result.TotalRecords, result.Data, endpointUrl);
    }
}
