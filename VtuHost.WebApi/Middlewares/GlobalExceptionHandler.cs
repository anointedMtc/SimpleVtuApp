using Identity.Application.Exceptions;
using Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Exceptions;
using SharedKernel.Domain.Exceptions;
using VtuApp.Application.Exceptions;
using VtuApp.Domain.Exceptions;
using Wallet.Application.Exceptions;
using Wallet.Domain.Exceptions;

namespace VtuHost.WebApi.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, 
        IHostEnvironment env, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _env = env;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        httpContext.Response.ContentType = "application/json";

        //httpContext.Response.StatusCode = exception switch
        var statusCode = exception switch
        {
            // sharedKernel.Domain
            InvalidAmountException => StatusCodes.Status400BadRequest,
            // sharedKernel.Application
            BadRequestException => StatusCodes.Status400BadRequest,
            ForbiddenAccessException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            // Wallet.Domain
            InsufficientWalletFundsException => StatusCodes.Status400BadRequest,
            InvalidTransferAmountException => StatusCodes.Status400BadRequest,
            WalletNotFoundException => StatusCodes.Status404NotFound,
            // Wallet.Application
            OwnerAlreadyExistsException => StatusCodes.Status400BadRequest,
            WalletAlreadyExistsException => StatusCodes.Status400BadRequest,
            // VtuApp.Domain
            InsufficientCustomerFundsException => StatusCodes.Status400BadRequest,
            // VtuApp.Application
            CustomerAlreadyExistsException => StatusCodes.Status400BadRequest,
            UnrecognisedDataPlanException => StatusCodes.Status400BadRequest,
            UnrecognisedDataPlanPriceException => StatusCodes.Status400BadRequest,
            UnrecognisedNetworkProviderException => StatusCodes.Status400BadRequest,
            // Identity.Domain
            RoleNotFoundException => StatusCodes.Status400BadRequest,
            UserNotFoundException => StatusCodes.Status400BadRequest,
            // Identity.Application
            CustomBadRequestException => StatusCodes.Status400BadRequest,
            CustomForbiddenException => StatusCodes.Status403Forbidden,
            CustomInternalServerException => StatusCodes.Status500InternalServerError,
            CustomNotFoundException => StatusCodes.Status404NotFound,
            CustomUnauthorizedException => StatusCodes.Status401Unauthorized,

            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            
            _ => StatusCodes.Status500InternalServerError
        };

        var message = exception switch
        {
            // sharedKernel.Domain
            InvalidAmountException => "The amount must be greater than 0",
            // sharedKernel.Application
            BadRequestException => "You have made a bad request",
            ForbiddenAccessException => "Forbidden. You are not allowed to access this resource",
            NotFoundException => "The specified resource was not found.",
            ValidationException => "Some inputs are not correct or in the valid format",
            // Wallet.Domain
            InsufficientWalletFundsException => "You do not have sufficient balance in your wallet to proceed with this transaction",
            InvalidTransferAmountException => "The transfer amount must be greater than 0",
            WalletNotFoundException => "You have made a bad request",
            // Wallet.Application
            OwnerAlreadyExistsException => "You have made a bad request",
            WalletAlreadyExistsException => "You have made a bad request",
            // VtuApp.Domain
            InsufficientCustomerFundsException => "You do not have sufficient vtu-balance to proceed with this operation",
            // VtuApp.Application
            CustomerAlreadyExistsException => "You have made a bad request",
            UnrecognisedDataPlanException => "The data plan chosen is not correct or in the list of valid categories",
            UnrecognisedDataPlanPriceException => "Internal Server Error",
            UnrecognisedNetworkProviderException => "The network provider chosen is not correct or in the list of valid categories",
            // Identity.Domain
            RoleNotFoundException => "You have made a bad request",
            UserNotFoundException => "You have made a bad request",
            // Identity.Application
            CustomBadRequestException => "You have made a bad request",
            CustomForbiddenException => "Forbidden. You are not allowed to access this resource",
            CustomInternalServerException => "There occured an internal server error",
            CustomNotFoundException => "The specified resource was not found.",
            CustomUnauthorizedException => exception.Message,

            UnauthorizedAccessException => "You are not authorized to access this endpoint",

            _ => "There occured an internal server error"       
        };

        var title = statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Not Found",
            _ => "Internal Server Error"
        };

        var instance = statusCode switch
        {
            StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            StatusCodes.Status401Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            StatusCodes.Status403Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };


        if (exception is ValidationException validationAppException)
        {
            if (_env.IsDevelopment() || _env.IsStaging())
            {
                // this is a new annonymous method
                await httpContext.Response.WriteAsJsonAsync(
                    new
                    {
                        httpContext.Response.StatusCode,
                        message,
                        Success = false,
                        ValidationErrors = validationAppException.Errors,

                    }, cancellationToken: cancellationToken);

                return true;
            }

            // it would also return a boolean value... and the correct content/type (problem/json)
            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Title = title,
                    Status = statusCode,
                    Detail = validationAppException.Errors.ToString(),
                    Type = exception.GetType().Name,
                    Instance = instance,
                },
                Exception = exception,
            });
        }


        if (_env.IsDevelopment() || _env.IsStaging())
        {
            problemDetails.Title = message;
            problemDetails.Status = httpContext.Response.StatusCode;
            problemDetails.Type = exception.GetType().Name;
            problemDetails.Detail = exception.StackTrace?.ToString();
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            // it would also return a boolean value... and the correct content/type (problem/json)
            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Title = title,
                    Status = statusCode,
                    Detail = message,
                    Type = exception.GetType().Name,
                    Instance = instance,
                },
                Exception = exception,
            });
        }
        _logger.LogError(exception, "Something went wrong: {Message}", exception.Message);

        return true;
    }
}
