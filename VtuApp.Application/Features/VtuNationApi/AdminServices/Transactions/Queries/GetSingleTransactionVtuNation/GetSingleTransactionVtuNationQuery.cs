using MediatR;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetSingleTransactionVtuNation;

public sealed class GetSingleTransactionVtuNationQuery : IRequest<GetSingleTransactionVtuNationResponse>
{
    public string Id { get; set; }
}
