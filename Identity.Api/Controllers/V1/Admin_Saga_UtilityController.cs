using Identity.Application.Features.Utilities.TriggerUserCreatedEventSaga;

namespace Identity.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public sealed class Admin_Saga_UtilityController : ApiBaseController
{
    [HttpPost("trigger-user-created-event-saga")]
    public async Task<ActionResult<TriggerUserCreatedEventSagaResponse>> TriggerUserCreatedEventSaga(string userEmail)
    {
        var result = await Mediator.Send(new TriggerUserCreatedEventSagaCommand() { UserEmail = userEmail });

        return Ok(result);
    }

}
