namespace VtuApp.Domain.Exceptions;

public class InsufficientCustomerFundsException : Exception
{
    public Guid WalletId { get; }

    public InsufficientCustomerFundsException(Guid walletId)
        : base($"Insufficient funds for wallet with ID: '{walletId}'.")
    {
        WalletId = walletId;
    }
}
