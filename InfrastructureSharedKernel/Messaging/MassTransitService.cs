using ApplicationSharedKernel.Interfaces;
using MassTransit;

namespace InfrastructureSharedKernel.Messaging;

public class MassTransitService : IMassTransitService
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPublishEndpoint _publishEndpoint;

    
    public MassTransitService(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Send<T>(string destinationAddress, T message) where T : class
    {
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{destinationAddress}"));

        await sendEndpoint.Send(message);
    }

    public async Task Publish<T>(T message) where T : class
    {
        await _publishEndpoint.Publish(message);
    }


}
