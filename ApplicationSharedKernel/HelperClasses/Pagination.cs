using Microsoft.AspNetCore.Http;
using SharedKernel.Domain.HelperClasses;

namespace ApplicationSharedKernel.HelperClasses;

public class Pagination<T> where T : class
{
    private readonly IHttpContextAccessor _contextAccessor;

    // in the real sense it should not have most of these constructors again... the only constructor required now is the one that takes in PaginationFilter
    public Pagination(int pageNumber, int pageSize, int totalRecords, T data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        Data = data;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }

    public Pagination()
    {

    }


    public int PageNumber { get; set; }          // or PageIndex
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; init; }
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public T Data { get; set; }


    public string PreviousPage { get; private set; } = string.Empty;
    public string NextPage { get; private set; } = string.Empty;
    public string FirstPage { get; private set; } = string.Empty;
    public string LastPage { get; private set; } = string.Empty;


    private const int PageNumbOne = 1;



    public Pagination(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }


    public Pagination(PaginationFilter paginationFilter, int totalRecords, T data)
    {
        PageNumber = paginationFilter.PageNumber;
        PageSize = paginationFilter.PageSize;
        TotalRecords = totalRecords;
        Data = data;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)paginationFilter.PageSize);

        var requestEndpointUrl = $"{_contextAccessor?.HttpContext.Request.Scheme}://{_contextAccessor?.HttpContext.Request.Host}{_contextAccessor?.HttpContext.Request.Path.Value}";


        var queryNextPage = new Dictionary<string, string?>
            {
                { "Search", paginationFilter.Search?.ToString() },
                { "Sort", paginationFilter.Sort?.ToString() },
                { "PageNumber", (paginationFilter.PageNumber + 1).ToString() },
                { "PageSize", paginationFilter.PageSize.ToString() },
            };

        var queryPreviousPage = new Dictionary<string, string?>
            {
                { "Search", paginationFilter.Search?.ToString() },
                { "Sort", paginationFilter.Sort?.ToString() },
                { "PageNumber", (paginationFilter.PageNumber - 1).ToString() },
                { "PageSize", paginationFilter.PageSize.ToString() },
            };

        var queryFirstPage = new Dictionary<string, string?>
            {
                { "Search", paginationFilter.Search?.ToString() },
                { "Sort", paginationFilter.Sort?.ToString() },
                { "PageNumber", PageNumbOne.ToString() },
                { "PageSize", paginationFilter.PageSize.ToString() },
            };

        var queryLastPage = new Dictionary<string, string?>
            {
                { "Search", paginationFilter.Search?.ToString() },
                { "Sort", paginationFilter.Sort?.ToString() },
                { "PageNumber", TotalPages.ToString() },
                { "PageSize", paginationFilter.PageSize.ToString() },
            };




        NextPage = paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < TotalPages
        ? QueryStringHelper.BuildPageUrl(requestEndpointUrl, queryNextPage) : null!;

        PreviousPage = paginationFilter.PageNumber - 1 >= 1 && paginationFilter.PageNumber <= TotalPages
            ? QueryStringHelper.BuildPageUrl(requestEndpointUrl, queryPreviousPage) : null!;

        FirstPage = QueryStringHelper.BuildPageUrl(requestEndpointUrl, queryFirstPage);

        LastPage = paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < TotalPages
            ? QueryStringHelper.BuildPageUrl(requestEndpointUrl, queryLastPage) : null!;

    }
}
