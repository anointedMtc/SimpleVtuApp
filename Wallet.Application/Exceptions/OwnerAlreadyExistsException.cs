namespace Wallet.Application.Exceptions;

public sealed class OwnerAlreadyExistsException : Exception
{
    public string Email { get; }

    public OwnerAlreadyExistsException(string email) : base($"Owner with email: '{email}' already exists.")
    {
        Email = email;
    }
}
