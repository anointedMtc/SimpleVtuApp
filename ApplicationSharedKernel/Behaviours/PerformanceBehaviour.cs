using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Interfaces;
using System.Diagnostics;

namespace SharedKernel.Application.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IUserContext _userContext;

    public PerformanceBehaviour(ILogger<TRequest> logger, IUserContext userContext)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _userContext = userContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;


        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            //var userId = _userContext.GetCurrentUser()?.Id ?? string.Empty;
            var userId = _userContext.GetCurrentUser()?.Id ?? $"anonymous User";
            var userName = string.Empty;

            _logger.LogWarning("Order Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName, request);

        }

        return response;
    }
}
