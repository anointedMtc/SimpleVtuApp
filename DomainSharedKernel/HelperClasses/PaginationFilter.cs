namespace SharedKernel.Domain.HelperClasses;

public class PaginationFilter
{

    public string? Search { get; set; }

    public string? Sort { get; set; }


    private const int MaxPageSize = 50; // so that users can't cheat by requiring a very large amount that can slow our Api down
    private const int MinPageNumber = 1;
    // public int PageIndex { get; set; } = 1; // start at page 1 if no page index is provided

    private int _pageSize = 7; // 7 products per page for a start... this is a field...
    private int _pageNumber = 1;

    public int PageNumber    // another way of doing this is using the constructor
    {
        get => _pageNumber;
        set => _pageNumber = value < MinPageNumber ? MinPageNumber : value;
    }
    public int PageSize    // this is the property... we can set additional behaviours here
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;  // we don't want our users to cheat... so if they request for a number greater than 50 which is the maximum we want to allow per page, it should ignore it and return 50... but if it is lower, then it should return the number requested
    }



    // here we're adding parameters for the sort we did b4 so we can use this class to replace them
    //public int? BrandId { get; set; }
    //public int? TypeId { get; set; }






}