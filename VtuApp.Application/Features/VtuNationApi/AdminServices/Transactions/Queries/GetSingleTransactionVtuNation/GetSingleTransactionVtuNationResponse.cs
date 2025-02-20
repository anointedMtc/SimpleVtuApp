using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Transaction;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetSingleTransactionVtuNation;

public sealed class GetSingleTransactionVtuNationResponse : ApiBaseResponse
{
    public GetSingleTransactionResponseVtuNation? GetSingleTransactionResponseVtuNation { get; set; }
}
