﻿using Identity.Domain.Entities;
using SharedKernel.Domain.HelperClasses;

namespace Identity.Application.Specifications;

public class ApplicationRoleSpecification : BaseSpecification<ApplicationRole>
{
    public ApplicationRoleSpecification(PaginationFilter paginationFilterAppUser)
    : base(x =>
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.Id.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.Name!.ToLower().Contains(paginationFilterAppUser.Search)) ||
            (string.IsNullOrEmpty(paginationFilterAppUser.Search) || x.Description!.ToLower().Contains(paginationFilterAppUser.Search))
    )
    {

        if (!string.IsNullOrEmpty(paginationFilterAppUser.Sort))
        {
            switch (paginationFilterAppUser.Sort)
            {
                // you can add as many sorting choices as you may want here

                case "nameAsc":
                    ApplyOrderBy(p => p.Name!);
                    break;
                case "nameDesc":
                    ApplyOrderByDescending(p => p.Name!);
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
