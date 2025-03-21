﻿using FluentValidation;

namespace VtuApp.Application.Features.VtuNationApi.UserServices.Commands.BuyDataVtuNation.Mtn.Buy500MB;

internal sealed class Buy500MBVtuNationValidator : AbstractValidator<Buy500MBVtuNationCommand>
{
    public Buy500MBVtuNationValidator()
    {

        RuleFor(r => r.PhoneNumber)
           .NotEmpty().WithMessage("{PropertyName} should have value.")
           .MinimumLength(11).WithMessage("{PropertyName} should me minimum of {ComparisonValue}. {PropertyValue} does not meet requirement.");

    }
}
