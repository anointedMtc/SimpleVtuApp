using SharedKernel.Application.DTO;
using VtuApp.Shared.DTO;

namespace VtuApp.Application.Features.Queries.GetCustomerAndBonusTransfersAndVtuTransactions;

public sealed class GetCustomerAndBonusTransfersAndVtuTransactionsResponse
    : ApiBaseResponse
{
    public CustomerDto CustomerDto { get; set; }
}
