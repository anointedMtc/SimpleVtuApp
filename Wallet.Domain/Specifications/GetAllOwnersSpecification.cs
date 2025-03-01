using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities;

namespace Wallet.Domain.Specifications;

public sealed class GetAllOwnersSpecification : BaseSpecification<Owner>
{
    public GetAllOwnersSpecification(PaginationFilter paginationFilter)
         : base(x =>
             (string.IsNullOrEmpty(paginationFilter.Search) || x.OwnerId.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.ApplicationUserId.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.Email.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.FirstName.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.LastName.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.CreatedAt.ToString().ToLower().Contains(paginationFilter.Search))
         )
    {
        if (!string.IsNullOrEmpty(paginationFilter.Sort))
        {
            switch (paginationFilter.Sort)
            {
                // you can add as many sorting choices as you may want here

                case "ownerIdAsc":
                    ApplyOrderBy(p => p.OwnerId);
                    break;
                case "ownerIdDesc":
                    ApplyOrderByDescending(p => p.OwnerId);
                    break;
                case "appUserIdAsc":
                    ApplyOrderBy(p => p.ApplicationUserId);
                    break;
                case "appUserIdDesc":
                    ApplyOrderByDescending(p => p.ApplicationUserId);
                    break;
                case "emailAsc":
                    ApplyOrderBy(p => p.Email!);
                    break;
                case "emailDesc":
                    ApplyOrderByDescending(p => p.Email!);
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
                case "dateAsc":
                    ApplyOrderBy(p => p.CreatedAt);
                    break;
                case "dateDesc":
                    ApplyOrderByDescending(p => p.CreatedAt);
                    break;

                default:
                    ApplyOrderBy(n => n.OwnerId);
                    break;

            }
        }
        else
        {
            ApplyOrderBy(n => n.OwnerId);
        }

        ApplyPaging(paginationFilter.PageNumber, paginationFilter.PageSize);

    }
}
