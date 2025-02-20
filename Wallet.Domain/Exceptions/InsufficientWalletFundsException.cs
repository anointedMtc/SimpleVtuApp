namespace Wallet.Domain.Exceptions;

public class InsufficientWalletFundsException : Exception
{
    public Guid WalletId { get; }

    public InsufficientWalletFundsException(Guid walletId)
        : base($"Insufficient funds for wallet with ID: '{walletId}'.")
    {
        WalletId = walletId;
    }
}
