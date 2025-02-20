using FluentValidation;

namespace SharedKernel.Application.Features.Commands.RemoveCache;

internal sealed class RemoveCacheValidator : AbstractValidator<RemoveCacheCommand>
{
    public RemoveCacheValidator()
    {
        RuleFor(r => r.CacheKey)
         .NotEmpty().WithMessage("{PropertyName} should have value");

    }
}
