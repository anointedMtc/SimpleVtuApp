using SagaOrchestrationStateMachines.Infrastructure.VtuDataOrderedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Domain.Specifications.VtuDataSaga;

public sealed class GetAllVtuDataSagaOrchestratorInstanceSpecification
    : BaseSpecification<VtuDataOrderedSagaStateInstance>
{
    public GetAllVtuDataSagaOrchestratorInstanceSpecification(PaginationFilter paginationFilter)
        : base(x =>
            string.IsNullOrEmpty(paginationFilter.Search) || x.CorrelationId.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.CurrentState.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.ApplicationUserId.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.Email.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.FirstName.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.LastName.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.VtuTransactionId.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.NetworkProvider.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.DataPlanPurchased.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.AmountToPurchase.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.PricePaid.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.Receiver.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.Sender.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.InitialBalance.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.FinalBalance.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.CreatedAt.ToString().Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase) ||
            string.IsNullOrEmpty(paginationFilter.Search) || x.SecondRetryVtuDataScheduleEventTokenId.ToString()!.Contains(paginationFilter.Search, StringComparison.OrdinalIgnoreCase)
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
                case "vtuTransactionIdAsc":
                    ApplyOrderBy(p => p.VtuTransactionId);
                    break;
                case "vtuTransactionIdDesc":
                    ApplyOrderByDescending(p => p.VtuTransactionId);
                    break;
                case "networkProviderAsc":
                    ApplyOrderBy(p => p.NetworkProvider!);
                    break;
                case "networkProviderDesc":
                    ApplyOrderByDescending(p => p.NetworkProvider!);
                    break;
                case "dataPlanPurchasedAsc":
                    ApplyOrderBy(p => p.DataPlanPurchased);
                    break;
                case "dataPlanPurchasedDesc":
                    ApplyOrderByDescending(p => p.DataPlanPurchased);
                    break;
                case "amountToPurchaseAsc":
                    ApplyOrderBy(p => p.AmountToPurchase);
                    break;
                case "amountToPurchaseDesc":
                    ApplyOrderByDescending(p => p.AmountToPurchase);
                    break;
                case "pricePaidAsc":
                    ApplyOrderBy(p => p.PricePaid);
                    break;
                case "pricePaidDesc":
                    ApplyOrderByDescending(p => p.PricePaid);
                    break;
                case "receiverAsc":
                    ApplyOrderBy(p => p.Receiver);
                    break;
                case "receiverDesc":
                    ApplyOrderByDescending(p => p.Receiver);
                    break;
                case "senderAsc":
                    ApplyOrderBy(p => p.Sender);
                    break;
                case "senderDesc":
                    ApplyOrderByDescending(p => p.Sender);
                    break;
                case "initialBalanceAsc":
                    ApplyOrderBy(p => p.InitialBalance);
                    break;
                case "initialBalanceDesc":
                    ApplyOrderByDescending(p => p.InitialBalance);
                    break;
                case "finalBalanceAsc":
                    ApplyOrderBy(p => p.FinalBalance);
                    break;
                case "finalBalanceDesc":
                    ApplyOrderByDescending(p => p.FinalBalance);
                    break;
                case "createdAtAsc":
                    ApplyOrderBy(p => p.CreatedAt);
                    break;
                case "createdAtDesc":
                    ApplyOrderByDescending(p => p.CreatedAt);
                    break;
                case "secondRetryAsc":
                    ApplyOrderBy(p => p.SecondRetryVtuDataScheduleEventTokenId!);
                    break;
                case "secondRetryDesc":
                    ApplyOrderByDescending(p => p.SecondRetryVtuDataScheduleEventTokenId!);
                    break;

                default:
                    ApplyOrderBy(n => n.CorrelationId);
                    break;

            }
        }

        ApplyPaging(paginationFilter.PageNumber, paginationFilter.PageSize);
    }
}
