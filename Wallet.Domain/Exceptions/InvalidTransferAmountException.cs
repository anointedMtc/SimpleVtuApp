namespace Wallet.Domain.Exceptions;

public class InvalidTransferAmountException : Exception
{
    public decimal Amount { get; }

    public InvalidTransferAmountException(decimal amount) : base($"Transfer has invalid amount: '{amount}'.")
    {
        Amount = amount;
    }
}
