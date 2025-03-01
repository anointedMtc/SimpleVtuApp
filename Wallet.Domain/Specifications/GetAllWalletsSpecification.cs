using SharedKernel.Domain.HelperClasses;
using Wallet.Domain.Entities.WalletAggregate;

namespace Wallet.Domain.Specifications;

public sealed class GetAllWalletsSpecification : BaseSpecification<WalletDomainEntity>
{
    public GetAllWalletsSpecification(PaginationFilter paginationFilter)
         : base(x =>
             (string.IsNullOrEmpty(paginationFilter.Search) || x.WalletDomainEntityId.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.OwnerId.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.ApplicationUserId.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.Email.ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.CreatedAt.ToString().ToLower().Contains(paginationFilter.Search)) ||
             (string.IsNullOrEmpty(paginationFilter.Search) || x.WalletBalance.ToString().ToLower().Contains(paginationFilter.Search)) 
         )
    {
        if (!string.IsNullOrEmpty(paginationFilter.Sort))
        {
            switch (paginationFilter.Sort)
            {
                // you can add as many sorting choices as you may want here

                case "walletIdAsc":
                    ApplyOrderBy(p => p.WalletDomainEntityId);
                    break;
                case "walletIdDesc":
                    ApplyOrderByDescending(p => p.WalletDomainEntityId);
                    break;
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
                case "dateAsc":
                    ApplyOrderBy(p => p.CreatedAt);
                    break;
                case "dateDesc":
                    ApplyOrderByDescending(p => p.CreatedAt);
                    break;
                case "walletBalAsc":
                    ApplyOrderBy(p => p.WalletBalance!);
                    break;
                case "walletBalDesc":
                    ApplyOrderByDescending(p => p.WalletBalance!);
                    break;
               

                default:
                    ApplyOrderBy(n => n.WalletDomainEntityId);
                    break;

            }
        }
        else
        {
            ApplyOrderBy(n => n.WalletDomainEntityId);
        }

        ApplyPaging(paginationFilter.PageNumber, paginationFilter.PageSize);

    }
}
