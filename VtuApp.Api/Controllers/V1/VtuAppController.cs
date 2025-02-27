using VtuApp.Application.Features.Commands.DeleteVtuCustomer;

namespace VtuApp.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class VtuAppController : ApiBaseController
{
    [HttpDelete("delete-vtuApp-customer")]
    public async Task<ActionResult<DeleteVtuCustomerResponse>> DeleteVtuCustomer([FromBody] DeleteVtuCustomerCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

}
