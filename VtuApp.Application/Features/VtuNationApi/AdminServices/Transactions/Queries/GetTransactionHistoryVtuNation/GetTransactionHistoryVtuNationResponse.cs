using ApplicationSharedKernel.DTO;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Transaction;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetTransactionHistoryVtuNation;

public sealed class GetTransactionHistoryVtuNationResponse : ApiBaseResponse
{
    public GetTransactionHistoryResponseVtuNation? GetTransactionHistoryResponseVtuNation { get; set; }
}
