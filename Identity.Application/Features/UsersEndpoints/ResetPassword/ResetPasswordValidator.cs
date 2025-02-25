using FluentValidation;
using SharedKernel.Application.Interfaces;

namespace Identity.Application.Features.UsersEndpoints.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    private readonly IUserContext _userContext;
    public ResetPasswordValidator(IUserContext userContext)
    {
        _userContext = userContext;
        var user = _userContext.GetCurrentUser();


        RuleFor(r => r.ResetPasswordDto.NewPassword)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .MinimumLength(7).WithMessage("{PropertyName} must be at least 7 characters long. {PropertyValue} does not meet requirements");

        RuleFor(r => r.ResetPasswordDto.ConfirmPassword)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .Equal(r => r.ResetPasswordDto.NewPassword).WithMessage("Passwords do not match!")
            .When(r => !string.IsNullOrWhiteSpace(r.ResetPasswordDto.NewPassword));

        RuleFor(e => e.ResetPasswordDto.Token)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

        RuleFor(e => e.ResetPasswordDto.Email)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .EmailAddress().WithName("MailId").WithMessage("{PropertyName} is invalid! {PropertyValue} does not meet requirements. Please check!");

    }


}
