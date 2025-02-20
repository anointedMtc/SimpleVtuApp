using FluentValidation.Results;

namespace ApplicationSharedKernel.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }



    // my own trial - but i changed my mind considering the fact that the Fluent Validation is actually returning an IEnumerable and not a Dictionary... but in my own implementation, it offered me Explicit Casting... check validation behaviour for more info...
    //public ValidationException(IDictionary<string, string[]> errors)
    //     : base()
    //{
    //    Errors = errors;
    //}
}



/*
 public IReadOnlyDictionary<string, string[]> Errors { get; }

        //public ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
        //    : base("One or more validation errors occured")
        //{
        //    Errors = errors;
        //}

        public ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
          : base()
        {
            Errors = errors;
        } 
*/