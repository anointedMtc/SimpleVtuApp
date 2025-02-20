using Identity.Application.Features.Utilities.TriggerUserCreatedEventSaga;

namespace Identity.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public sealed class IdentityUtilityController : ApiBaseController
{
    [HttpGet("trigger-user-created-event-saga")]
    public async Task<ActionResult<TriggerUserCreatedEventSagaResponse>> TriggerUserCreatedEventSaga(string userEmail)
    {
        var result = await Mediator.Send(new TriggerUserCreatedEventSagaCommand() { UserEmail = userEmail });

        return Ok(result);
    }




}
