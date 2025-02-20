namespace Identity.Application.Exceptions;

public class CustomInternalServerException : ApplicationException
{
    public CustomInternalServerException(string message) : base(message)
    {

    }

    public CustomInternalServerException()
    {

    }
}
