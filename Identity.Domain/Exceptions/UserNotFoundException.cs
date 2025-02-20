namespace Identity.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException()
    {
        
    }
    public UserNotFoundException(string applicationUserId) : base($"Bad Request. User with Id {applicationUserId} was not found.")
    {
        
    }
}
