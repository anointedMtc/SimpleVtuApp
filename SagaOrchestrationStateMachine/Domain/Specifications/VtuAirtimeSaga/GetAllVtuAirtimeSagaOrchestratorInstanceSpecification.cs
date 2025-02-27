using SagaOrchestrationStateMachines.Infrastructure.VtuAirtimeOrderedSagaOrchestrator;
using SharedKernel.Domain.HelperClasses;

namespace SagaOrchestrationStateMachines.Domain.Specifications.VtuAirtimeSaga;

public sealed class GetAllVtuAirtimeSagaOrchestratorInstanceSpecification
    : BaseSpecification<VtuAirtimeOrderedSagaStateInstance>
{
    public GetAllVtuAirtimeSagaOrchestratorInstanceSpecification(PaginationFilter paginationFilter)
        : base(x =>
            (string.IsNullOrEmpty(paginationFilter.Search) || x.CorrelationId.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.CurrentState.ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.ApplicationUserId.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.Email.ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.FirstName.ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.LastName.ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.VtuTransactionId.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.NetworkProvider.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.AmountToPurchase.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.PricePaid.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.Receiver.ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.Sender.ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.InitialBalance.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.FinalBalance.ToString().ToLower().Contains(paginationFilter.Search)) ||
            (string.IsNullOrEmpty(paginationFilter.Search) || x.CreatedAt.ToString().ToLower().Contains(paginationFilter.Search)) 
            //(string.IsNullOrEmpty(paginationFilter.Search) || x.SecondRetryVtuAirtimeScheduleEventTokenId.ToString()!.Contains(paginationFilter.Search)) 
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
                    ApplyOrderBy(p => p.SecondRetryVtuAirtimeScheduleEventTokenId!);
                    break;
                case "secondRetryDesc":
                    ApplyOrderByDescending(p => p.SecondRetryVtuAirtimeScheduleEventTokenId!);
                    break;

                default:
                    ApplyOrderBy(n => n.CorrelationId);
                    break;

            }
        }
        else
        {
            ApplyOrderBy(n => n.CorrelationId);
        }

        ApplyPaging(paginationFilter.PageNumber, paginationFilter.PageSize);
    }
}
