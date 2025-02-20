namespace Wallet.Domain.Exceptions;

public class WalletNotFoundException : Exception
{
    public Guid WalletId { get; }

    public WalletNotFoundException(Guid walletId) : base($"Wallet with ID: '{walletId}' was not found.")
    {
        WalletId = walletId;
    }
}
