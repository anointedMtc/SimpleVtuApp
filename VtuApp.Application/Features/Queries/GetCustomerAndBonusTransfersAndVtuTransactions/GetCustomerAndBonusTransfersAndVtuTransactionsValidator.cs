using FluentValidation;

namespace VtuApp.Application.Features.Queries.GetCustomerAndBonusTransfersAndVtuTransactions;

internal sealed class GetCustomerAndBonusTransfersAndVtuTransactionsValidator
    : AbstractValidator<GetCustomerAndBonusTransfersAndVtuTransactionsQuery>
{
    public GetCustomerAndBonusTransfersAndVtuTransactionsValidator()
    {
        RuleFor(r => r.Email)
          .NotEmpty().WithMessage("{PropertyName} should have value. ");

    }
}
