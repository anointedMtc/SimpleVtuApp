using FluentValidation;

namespace Wallet.Application.Features.Commands.DeleteOwner;

internal sealed class DeleteOwnerValidator : AbstractValidator<DeleteOwnerCommand>
{
    public DeleteOwnerValidator()
    {
        RuleFor(e => e.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

    }
}
