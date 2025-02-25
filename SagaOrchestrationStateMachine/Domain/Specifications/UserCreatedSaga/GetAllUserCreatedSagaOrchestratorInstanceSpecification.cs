using SagaOrchestrationStateMachines.Infrastructure.UserCreatedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Domain.Specifications.UserCreatedSaga;

public sealed class GetAllUserCreatedSagaOrchestratorInstanceSpecification
    : BaseSpecification<UserCreatedSagaStateInstance>
{
    public GetAllUserCreatedSagaOrchestratorInstanceSpecification(PaginationFilter paginationFilter)
        : base(x =>
            (string.IsNullOrEmpty(paginationFilter.Search) || x.CorrelationId.ToString().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.CurrentState.Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.ApplicationUserId.ToString().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.FirstName.Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.LastName.Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.Email.Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.PhoneNumber.Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.RegisterationBonus.ToString().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.CreatedAt.ToString().Contains(paginationFilter.Search)) 
            //(string.IsNullOrEmpty(paginationFilter.Search) || x.UserCreatedInAllModulesEventStatus.ToString().Contains(paginationFilter.Search)) 
            //(string.IsNullOrEmpty(paginationFilter.Search) || x.NotifyApplicationUserScheduleEventTokenId.ToString()!.Contains(paginationFilter.Search))
        )
    {
        if (!string.IsNullOrEmpty(paginationFilter.Sort))
        {
            switch (paginationFilter.Sort)
            {
                // you can add as many sorting choices as you may want here

                case "currentStateAsc":
                    ApplyOrderBy(p => p.CurrentState);
                    break;
                case "currentStateDesc":
                    ApplyOrderByDescending(p => p.CurrentState);
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
                case "registerationBonusAsc":
                    ApplyOrderBy(p => p.RegisterationBonus!);
                    break;
                case "registerationBonusDesc":
                    ApplyOrderByDescending(p => p.RegisterationBonus!);
                    break;
                case "createdAtAsc":
                    ApplyOrderBy(p => p.CreatedAt);
                    break;
                case "createdAtDesc":
                    ApplyOrderByDescending(p => p.CreatedAt);
                    break;
                case "userCreatedInAllModulesEventStatusAsc":
                    ApplyOrderBy(p => p.UserCreatedInAllModulesEventStatus!);
                    break;
                case "userCreatedInAllModulesEventStatusDesc":
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
