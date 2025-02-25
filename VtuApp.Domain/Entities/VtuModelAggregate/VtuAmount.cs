using SharedKernel.Domain;
using SharedKernel.Domain.Exceptions;
using System.Globalization;

namespace VtuApp.Domain.Entities.VtuModelAggregate;

public class VtuAmount : ValueObject
{
    public decimal Value { get; private set; }

    public VtuAmount(decimal value)
    {
        if (value is < 0 or > 1000000)
        {
            throw new InvalidAmountException(value);
        }

        Value = value;
    }


    public static VtuAmount Zero => new(0);

    // these are to help us perform automatic casting between decimal and amount ...back and forth

    public static implicit operator VtuAmount(decimal value) => new(value);

    public static implicit operator decimal(VtuAmount value) => value.Value;



    // we have already defined == and != operators in our base ValueObject class... we want to also specify specifically how we want the following operators to be applied

    public static bool operator >(VtuAmount a, VtuAmount b) => a.Value > b.Value;

    public static bool operator <(VtuAmount a, VtuAmount b) => a.Value < b.Value;

    public static bool operator >=(VtuAmount a, VtuAmount b) => a.Value >= b.Value;

    public static bool operator <=(VtuAmount a, VtuAmount b) => a.Value <= b.Value;

    public static VtuAmount operator +(VtuAmount a, VtuAmount b) => a.Value + b.Value;

    public static VtuAmount operator -(VtuAmount a, VtuAmount b) => a.Value - b.Value;


    // these are the properties I want to use for the structural comparison
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }


    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);


}
