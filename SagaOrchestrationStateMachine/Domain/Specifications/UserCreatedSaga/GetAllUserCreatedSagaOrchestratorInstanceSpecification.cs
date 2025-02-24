using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Domain.Specifications.UserCreatedSaga;

public sealed class GetAllUserCreatedSagaOrchestratorInstanceSpecification
    : BaseSpecification<UserCreatedSagaStateInstance>
{
    public GetAllUserCreatedSagaOrchestratorInstanceSpecification(PaginationFilter paginationFilter)
        : base(x =>
            string.IsNullOrEmpty(paginationFilter.Search) || x.CorrelationId.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.CurrentState.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.ApplicationUserId.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.FirstName.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.LastName.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.Email.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.PhoneNumber.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.RegisterationBonus.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.CreatedAt.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.UserCreatedInAllModulesEventStatus.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.NotifyApplicationUserScheduleEventTokenId.ToString()!.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase)
        )
    {
        if (!string.IsNullOrEmpty(paginationFilter.Sort))
        {
            switch (paginationFilter.Sort)
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
                    ApplyOrderBy(p => p.PhoneNumber);
                    break;
                case "constUserNameDesc":
                    ApplyOrderByDescending(p => p.PhoneNumber);
                    break;
                case "userNameAsc":
                    ApplyOrderBy(p => p.RegisterationBonus!);
                    break;
                case "userNameDesc":
                    ApplyOrderByDescending(p => p.RegisterationBonus!);
                    break;
                case "genderAsc":
                    ApplyOrderBy(p => p.CreatedAt);
                    break;
                case "genderDesc":
                    ApplyOrderByDescending(p => p.CreatedAt);
                    break;
                case "nationalityAsc":
                    ApplyOrderBy(p => p.UserCreatedInAllModulesEventStatus!);
                    break;
                case "nationalityDesc":
                    ApplyOrderByDescending(p => p.UserCreatedInAllModulesEventStatus!);
                    break;

                default:
                    ApplyOrderBy(n => n.CorrelationId);
                    break;

            }
        }

        ApplyPaging(paginationFilter.PageNumber, paginationFilter.PageSize);

    }
}
