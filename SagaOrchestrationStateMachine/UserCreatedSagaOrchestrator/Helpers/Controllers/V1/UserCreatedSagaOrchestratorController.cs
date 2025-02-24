//using Asp.Versioning;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Features.Queries;
//using SharedKernel.Api.Controllers;

//namespace SagaOrchestrationStateMachines.UserCreatedSagaOrchestrator.Helpers.Controllers.V1;

//[Authorize]
//[ApiVersion("1.0")]
//public class UserCreatedSagaOrchestratorController : ApiBaseController
//{
//    [HttpGet("get-user-created-saga-instance/{correlationId}")]
//    public async Task<ActionResult<GetUserCreatedSagOrchestratorInstanceResponse>> GetUserCreatedSagaInstance(Guid correlationId)
//    {
//        var result = await Mediator.Send(new GetUserCreatedSagOrchestratorInstanceQuery() { CorrelationId = correlationId });

//        return Ok(result);
//    }
//}
