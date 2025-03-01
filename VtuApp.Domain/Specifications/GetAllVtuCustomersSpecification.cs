using SharedKernel.Domain.HelperClasses;
using VtuApp.Domain.Entities.VtuModelAggregate;

namespace VtuApp.Domain.Specifications;
 
public sealed class GetAllVtuCustomersSpecification : BaseSpecification<Customer>
{
    public GetAllVtuCustomersSpecification(PaginationFilter paginationFilter)
         : base(x =>
             (string.IsNullOrEmpty(paginationFilter.Search) || x.CustomerId.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.ApplicationUserId.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.FirstName.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.LastName.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.Email.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.PhoneNumber.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.VtuBonusBalance.Value.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.TotalBalance.Value.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.NumberOfStars.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.TransactionCount.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.TimeLastStarWasAchieved.ToString().ToLower().Contains(paginationFilter.Search))
         )
    {
        if (!string.IsNullOrEmpty(paginationFilter.Sort))
        {
            switch (paginationFilter.Sort)
            {
                // you can add as many sorting choices as you may want here

                case "appUserIdAsc":
                    ApplyOrderBy(p => p.ApplicationUserId);
                    break;
                case "appUserIdDesc":
                    ApplyOrderByDescending(p => p.ApplicationUserId);
                    break;
                case "firstNameAsc":
                    ApplyOrderBy(p => p.FirstName);
                    break;
                case "firstNameDesc":
                    ApplyOrderByDescending(p => p.FirstName);
                    break;
                case "lastNameAsc":
                    ApplyOrderBy(p => p.LastName);
                    break;
                case "lastNameDesc":
                    ApplyOrderByDescending(p => p.LastName);
                    break;
                case "emailAsc":
                    ApplyOrderBy(p => p.Email!);
                    break;
                case "emailDesc":
                    ApplyOrderByDescending(p => p.Email!);
                    break;
                case "phoneNumberAsc":
                    ApplyOrderBy(p => p.PhoneNumber);
                    break;
                case "phoneNumberDesc":
                    ApplyOrderByDescending(p => p.PhoneNumber);
                    break;
                case "vtuBonusBalanceAsc":
                    ApplyOrderBy(p => p.VtuBonusBalance!);
                    break;
                case "vtuBonusBalanceDesc":
                    ApplyOrderByDescending(p => p.VtuBonusBalance!);
                    break;
                case "totalBalAsc":
                    ApplyOrderBy(p => p.TotalBalance);
                    break;
                case "totalBalDesc":
                    ApplyOrderByDescending(p => p.TotalBalance);
                    break;
                case "numberOfStarsAsc":
                    ApplyOrderBy(p => p.NumberOfStars!);
                    break;
                case "numberOfStarsDesc":
                    ApplyOrderByDescending(p => p.NumberOfStars!);
                    break;

                default:
                    ApplyOrderBy(n => n.CustomerId);
                    break;

            }
        }
        else
        {
            ApplyOrderBy(n => n.CustomerId);
        }

        ApplyPaging(paginationFilter.PageNumber, paginationFilter.PageSize);

    }
}
