using FluentValidation;

namespace Identity.Application.Features.UsersEndpoints.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(e => e.ChangePasswordRequestDto.CurrentPassword)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements");

        RuleFor(r => r.ChangePasswordRequestDto.NewPassword)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .MinimumLength(7).WithMessage("{PropertyName} must be at least 7 characters long. {PropertyValue} does not meet requirements");

        RuleFor(r => r.ChangePasswordRequestDto.ConfirmNewPassword)
            .NotEmpty().WithMessage("{PropertyName} should have value. {PropertyValue} does not meet requirements")
            .Equal(r => r.ChangePasswordRequestDto.NewPassword).WithMessage("Passwords do not match!")
            .When(r => !string.IsNullOrWhiteSpace(r.ChangePasswordRequestDto.NewPassword));

    }
}
