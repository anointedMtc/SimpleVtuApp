namespace VtuApp.Application.Exceptions;

public class UnrecognisedNetworkProviderException : Exception
{
    public string NetworkId { get; }
    public UnrecognisedNetworkProviderException(string networkId) : base($"The network provider {networkId} is not valid")
    {
        NetworkId = networkId;
    }
}
