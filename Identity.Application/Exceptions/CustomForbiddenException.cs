namespace Identity.Application.Exceptions;

public class CustomForbiddenException : ApplicationException
{
    public CustomForbiddenException(string message) : base(message)
    {

    }

    public CustomForbiddenException()
    {

    }
}
