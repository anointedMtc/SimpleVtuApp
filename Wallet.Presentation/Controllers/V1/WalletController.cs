using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Api.Controllers;
using SharedKernel.Application.HelperClasses;
using SharedKernel.Domain.HelperClasses;
using System.Net.Mime;
using Wallet.Application.Features.Commands.AddFunds;
using Wallet.Application.Features.Commands.DeductFunds;
using Wallet.Application.Features.Commands.DeleteOwner;
using Wallet.Application.Features.Commands.TransferFunds;
using Wallet.Application.Features.Queries.GetAllOwners;
using Wallet.Application.Features.Queries.GetAllWallets;
using Wallet.Application.Features.Queries.GetOwnerAndWalletByEmail;
using Wallet.Application.Features.Queries.GetWalletAndTransfersById;
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


    [HttpDelete("delete-wallet-Owner")]
    public async Task<ActionResult<DeleteOwnerResponse>> DeleteOwner([FromBody] DeleteOwnerCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }



    [HttpGet("get-all-owners")]
    public async Task<ActionResult<Pagination<GetAllOwnersResponse>>> GetAllOwners([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await Mediator.Send(new GetAllOwnersQuery(paginationFilter));

        var endpointUrl = $"{Request.Scheme}://{Request.Host}{Request.Path.Value}";

        return new Pagination<GetAllOwnersResponse>(paginationFilter, result.TotalRecords, result.Data, endpointUrl);
    }


    [HttpGet("get-all-wallets")]
    public async Task<ActionResult<Pagination<GetAllWalletsResponse>>> GetAllWallets([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await Mediator.Send(new GetAllWalletsQuery(paginationFilter));

        var endpointUrl = $"{Request.Scheme}://{Request.Host}{Request.Path.Value}";

        return new Pagination<GetAllWalletsResponse>(paginationFilter, result.TotalRecords, result.Data, endpointUrl);
    }


    [HttpGet("get-owner-and-wallet-by-email")]
    public async Task<ActionResult<GetOwnerAndWalletByEmailResponse>> GetOwnerAndWalletByEmail([FromQuery] GetOwnerAndWalletByEmailQuery command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


    [HttpGet("get-wallet-and-transfers-by-id")]
    public async Task<ActionResult<GetWalletAndTransfersByIdResponse>> GetWalletAndTransfersById([FromQuery] GetWalletAndTransfersByIdQuery command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }


}
