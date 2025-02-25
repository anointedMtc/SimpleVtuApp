using SharedKernel.Domain;
using SharedKernel.Domain.Exceptions;
using System.Globalization;

namespace Wallet.Domain.Entities.WalletAggregate;

public sealed class Amount : ValueObject
{
    public decimal Value { get; private set; }

    public Amount(decimal value)
    {
        if (value is < 0 or > 1000000)
        {
            throw new InvalidAmountException(value);
        }

        Value = value;
    }


    public static Amount Zero => new(0);

    // these are to help us perform automatic casting between decimal and amount ...back and forth

    public static implicit operator Amount(decimal value) => new(value);

    public static implicit operator decimal(Amount value) => value.Value;



    // we have already defined == and != operators in our base ValueObject class... we want to also specify specifically how we want the following operators to be applied

    public static bool operator >(Amount a, Amount b) => a.Value > b.Value;

    public static bool operator <(Amount a, Amount b) => a.Value < b.Value;

    public static bool operator >=(Amount a, Amount b) => a.Value >= b.Value;

    public static bool operator <=(Amount a, Amount b) => a.Value <= b.Value;

    public static Amount operator +(Amount a, Amount b) => a.Value + b.Value;

    public static Amount operator -(Amount a, Amount b) => a.Value - b.Value;


    // these are the properties I want to use for the structural comparison
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);


}
