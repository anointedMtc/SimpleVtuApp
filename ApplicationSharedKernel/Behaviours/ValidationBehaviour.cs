using FluentValidation;
using MediatR;

namespace ApplicationSharedKernel.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Behaviour One 
        //if (_validators.Any())
        //{
        //    var context = new ValidationContext<TRequest>(request);

        //    var validationResults = await Task.WhenAll(
        //        _validators.Select(v =>
        //            v.ValidateAsync(context, cancellationToken)));

        //    var failures = validationResults
        //        .Where(r => r.Errors.Any())
        //        .SelectMany(r => r.Errors)
        //        .ToList();

        //    if (failures.Any())
        //        throw new ValidationException(failures);
        //}
        //return await next();





        //// Behaviour Two
        //// 1. First we check if there are any validators attached to the incoming request
        //if (!_validators.Any())
        //    return await next();

        //// 2. if available, we create a new validation context using the request, and validat it. This would return the ValidationResults array.
        //// CodeMaze
        //var context = new ValidationContext<TRequest>(request);

        //var validationFailures = await Task.WhenAll(
        //    _validators.Select(validator => validator.ValidateAsync(context)));

        //// 3. From this array of results, we filter out the failures (where the error count is greater than 0)
        //var errorsDictionary = validationFailures
        //    .SelectMany(x => x.Errors)
        //    .Where(x => x is not null)
        //    .GroupBy(
        //        x => x.PropertyName.Substring(x.PropertyName.IndexOf('.') + 1),
        //        x => x.ErrorMessage, (propertyName, errorMessages) => new
        //        {
        //            Key = propertyName,
        //            Values = errorMessages.Distinct().ToArray()
        //        })
        //    .ToDictionary(x => x.Key, x => x.Values);

        //// 4. we throw this as an Exception and pass the errored out messages.
        ////if (errorsDictionary.Any())  // use count instead of any
        //if (errorsDictionary.Count != 0)  // use count instead of any
        //    throw new ValidationException((IEnumerable<FluentValidation.Results.ValidationFailure>)errorsDictionary);



        //// return await next().ConfigureAwait(false);   // likely addition from CodewithMukesh
        //return await next();




        // My own implementation
        // Behaviour One 
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Count != 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);
        }
        return await next();

    }

   
}

