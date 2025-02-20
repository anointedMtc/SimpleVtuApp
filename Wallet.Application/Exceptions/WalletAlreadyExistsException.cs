namespace Wallet.Application.Exceptions;

public sealed class WalletAlreadyExistsException : Exception
{
    public string Email { get; }

    public WalletAlreadyExistsException(string email) : base($"Owner with email: '{email}' already exists.")
    {
        Email = email;
    }
}
