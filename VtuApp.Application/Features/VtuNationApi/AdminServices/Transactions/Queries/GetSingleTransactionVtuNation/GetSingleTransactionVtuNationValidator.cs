using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.AdminServices.Transactions.Queries.GetSingleTransactionVtuNation;

internal sealed class GetSingleTransactionVtuNationValidator : AbstractValidator<GetSingleTransactionVtuNationQuery>
{
    public GetSingleTransactionVtuNationValidator()
    {
        RuleFor(r => r.Id)
         .NotEmpty().WithMessage("{PropertyName} should have value. `{PropertyValue}` does not meet requirements");

    }
}
