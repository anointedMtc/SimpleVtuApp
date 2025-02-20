using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace ApplicationSharedKernel.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // We first set a random GUID as the Correlation ID. This is to ensure both the request and responses can be tracked using the ID
        var correlationId = Guid.NewGuid();

        // Request Logging
        // Serialize the request
        var requestJson = JsonSerializer.Serialize(request);
        // log the serialized request
        //_logger.LogInformation("Handling request {CorrelationID}: {Request}", correlationId, requestJson);

        _logger.LogInformation("Handling request {CorrelationId}: {Request} for {RequestName} at {DateTimeUtc}",
            correlationId, requestJson, typeof(TRequest).Name, DateTimeOffset.UtcNow);

        var stopWatch = Stopwatch.StartNew();

        // Response logging
        var response = await next();
        // Serialize the request
        var responseJson = JsonSerializer.Serialize(response);
        // log the serialized request

        stopWatch.Stop();
        _logger.LogInformation("Response for {Correlation}: {Response} by {ResponseName} in {msTime} ms at {DateTimeUtc}",
            correlationId, responseJson, typeof(TResponse).Name, stopWatch.Elapsed, DateTimeOffset.UtcNow);

        // Return response
        return response;
    }
}
