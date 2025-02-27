using Identity.Domain.Entities;
using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.Specifications;

public class ApplicationUserSpecification : BaseSpecification<ApplicationUser>
{
    public ApplicationUserSpecification(PaginationFilter paginationFilterAppUser)
    : base(x =>
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.Id.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.FirstName.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.LastName.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.Email!.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.ConstUserName.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.UserName!.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.Gender.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.Nationality!.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.DateOfBirth.HasValue.ToString().ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.LastLogin.ToString().ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.UpdatedAt.ToString().ToLower().Contains(paginationFilterAppUser.Search))

    )
    {

        if (!string.IsNullOrEmpty(paginationFilterAppUser.Sort))
        {
            switch (paginationFilterAppUser.Sort)
            {
                // you can add as many sorting choices as you may want here

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
                case "constUserNameAsc":
                    ApplyOrderBy(p => p.ConstUserName);
                    break;
                case "constUserNameDesc":
                    ApplyOrderByDescending(p => p.ConstUserName);
                    break;
                case "userNameAsc":
                    ApplyOrderBy(p => p.UserName!);
                    break;
                case "userNameDesc":
                    ApplyOrderByDescending(p => p.UserName!);
                    break;
                case "genderAsc":
                    ApplyOrderBy(p => p.Gender);
                    break;
                case "genderDesc":
                    ApplyOrderByDescending(p => p.Gender);
                    break;
                case "nationalityAsc":
                    ApplyOrderBy(p => p.Nationality!);
                    break;
                case "nationalityDesc":
                    ApplyOrderByDescending(p => p.Nationality!);
                    break;
                
                default:
                    ApplyOrderBy(n => n.Id);
                    break; 

            }
        }
        else
        {
            ApplyOrderBy(n => n.Id);
        }

        ApplyPaging(paginationFilterAppUser.PageNumber, paginationFilterAppUser.PageSize);
    }
}
