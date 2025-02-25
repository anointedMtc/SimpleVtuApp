namespace SharedKernel.Application.Interfaces;

public interface IMassTransitService
{
    Task Send<T>(string destinationAddress, T message) where T : class;
    Task Publish<T>(T message) where T : class;
}
