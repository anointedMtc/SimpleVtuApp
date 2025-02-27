using FluentValidation;

namespace VtuApp.Application.Features.Commands.DeleteVtuCustomer;

internal sealed class DeleteVtuCustomerValidator : AbstractValidator<DeleteVtuCustomerCommand>
{
    public DeleteVtuCustomerValidator()
    {
        RuleFor(e => e.Email)
           .NotEmpty().WithMessage("{PropertyName} should have value")
           .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

    }
}
