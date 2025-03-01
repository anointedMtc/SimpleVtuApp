using SharedKernel.Application.HelperClasses;
using SharedKernel.Domain.HelperClasses;
using VtuApp.Application.Features.Commands.DeleteVtuCustomer;
using VtuApp.Application.Features.Commands.TransferVtuBonusToMainWallet;
using VtuApp.Application.Features.Queries.GetAllVtuCustomers;
using VtuApp.Application.Features.Queries.GetCustomerAndBonusTransfersAndVtuTransactions;

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


    [HttpGet("get-customer-and-bonusTransfers-and-vtuTransactions")] // use the other approach
    public async Task<ActionResult<GetCustomerAndBonusTransfersAndVtuTransactionsResponse>> GetCustomerWithBonusAndTransactionHistory([FromQuery] string email)
    {
        var result = await Mediator.Send(new GetCustomerAndBonusTransfersAndVtuTransactionsQuery() { Email = email});

        return Ok(result);
    }


    [HttpGet("get-all-vtuCustomers")]
    public async Task<ActionResult<Pagination<GetAllVtuCustomersResponse>>> GetAllVtuCustomers([FromQuery] PaginationFilter paginationFilter)
    {
        var result = await Mediator.Send(new GetAllVtuCustomersQuery(paginationFilter));

        var endpointUrl = $"{Request.Scheme}://{Request.Host}{Request.Path.Value}";

        return new Pagination<GetAllVtuCustomersResponse>(paginationFilter, result.TotalRecords, result.Data, endpointUrl);
    }



}
