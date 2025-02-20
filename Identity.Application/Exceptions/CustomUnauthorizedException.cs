namespace Identity.Application.Exceptions;

public class CustomUnauthorizedException : ApplicationException
{
    public CustomUnauthorizedException(string message) : base(message)
    {

    }

    public CustomUnauthorizedException()
    {

    }
}
