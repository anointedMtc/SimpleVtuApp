using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Api.Controllers;
using System.Net.Mime;
using Wallet.Application.Features.Commands.AddFunds;
using Wallet.Application.Features.Commands.DeductFunds;
using Wallet.Application.Features.Commands.TransferFunds;
using Wallet.Application.Features.Queries.GetWalletById;

namespace Wallet.Api.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
public class WalletController : ApiBaseController
{


    [HttpGet("getWallet/{walletId}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GetWalletByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GetWalletByIdResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GetWalletByIdResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetWalletByIdResponse>> GetWalletById(Guid walletId)
    {
        var result = await Mediator.Send(new GetWalletByIdQuery() { WalletId = walletId });

        return Ok(result);
    }


    [HttpPost("addFunds")]
    public async Task<ActionResult<AddFundsResponse>> AddFunds([FromBody] AddFundsCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


    [HttpPost("deductFunds")]
    public async Task<ActionResult<DeductFundsResponse>> DeductFunds([FromBody]  DeductFundsCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpPost("transferFunds")]
    public async Task<ActionResult<TransferFundsResponse>> TransferFunds([FromBody] TransferFundsCommand command)
    {
        return await Mediator.Send(command);
    }

}
