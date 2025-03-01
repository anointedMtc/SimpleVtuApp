using SharedKernel.Application.HelperClasses;
using SharedKernel.Application.Interfaces;
using SharedKernel.Domain.HelperClasses;
using VtuApp.Application.HelperClasses;

namespace VtuApp.Application.Features.Queries.GetAllVtuCustomers;

public sealed class GetAllVtuCustomersQuery : ICachedQuery<Pagination<GetAllVtuCustomersResponse>>
{
    public GetAllVtuCustomersQuery(PaginationFilter paginationFilter) : base()
    {
        PaginationFilter = paginationFilter;
    }

    public PaginationFilter PaginationFilter { get; set; }

    public string CacheKey => CacheHelperVtuApp.GenerateGetAllVtuCustomersCacheKey(PaginationFilter);

    public TimeSpan? Expiration => null;

}