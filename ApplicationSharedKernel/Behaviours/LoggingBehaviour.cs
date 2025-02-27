using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace SharedKernel.Application.Behaviours;

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
        var correlationId = Guid.NewGuid();

        var requestJson = JsonSerializer.Serialize(request);

        _logger.LogInformation("Handling request {CorrelationId}: {Request} for {RequestName} at {DateTimeUtc}",
            correlationId, requestJson, typeof(TRequest).Name, DateTimeOffset.UtcNow);

        var stopWatch = Stopwatch.StartNew();

        var response = await next();

        var responseJson = JsonSerializer.Serialize(response);

        stopWatch.Stop();
        _logger.LogInformation("Response for {Correlation}: {Response} by {ResponseName} in {msTime} ms at {DateTimeUtc}",
            correlationId, responseJson, typeof(TResponse).Name, stopWatch.Elapsed, DateTimeOffset.UtcNow);

        return response;
    }
}
