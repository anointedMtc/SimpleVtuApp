using VtuApp.Application.Features.Commands.DeleteVtuCustomer;
using VtuApp.Application.Features.Commands.TransferVtuBonusToMainWallet;

namespace VtuApp.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class VtuAppController : ApiBaseController
{
    [HttpPost("transfer-vtuBonus-to-wallet")]
    public async Task<ActionResult<TransferVtuBonusToMainWalletResponse>> TransferVtuBonusToWallet([FromBody] TransferVtuBonusToMainWalletCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


    [HttpDelete("delete-vtuApp-customer")]
    public async Task<ActionResult<DeleteVtuCustomerResponse>> DeleteVtuCustomer([FromBody] DeleteVtuCustomerCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

}
