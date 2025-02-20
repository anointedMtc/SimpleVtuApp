namespace SharedKernel.Domain.Exceptions;

public class InvalidAmountException : Exception
{
    public decimal Amount { get; }

    public InvalidAmountException(decimal amount) : base($"Amount: '{amount}' is invalid.")
    {
        Amount = amount;
    }
}
