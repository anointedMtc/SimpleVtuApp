using SharedKernel.Application.Interfaces;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.Queries.GetCustomerAndBonusTransfersAndVtuTransactions;

public sealed class GetCustomerAndBonusTransfersAndVtuTransactionsQuery
    : ICachedQuery<GetCustomerAndBonusTransfersAndVtuTransactionsResponse>
{
    public string Email { get; set; }

    public string CacheKey => CacheHelperVtuApp.GenerateGetCustomerAndBonusTransfersAndVtuTransactionsQueryCacheKey(Email);

    public TimeSpan? Expiration => null;
}
