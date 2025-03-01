using MediatR;

namespace VtuApp.Application.Features.Queries.GetCustomerAndBonusTransfersAndVtuTransactions;

public sealed class GetCustomerAndBonusTransfersAndVtuTransactionsQuery
    : IRequest<GetCustomerAndBonusTransfersAndVtuTransactionsResponse>
{
    public string Email { get; set; }
}
