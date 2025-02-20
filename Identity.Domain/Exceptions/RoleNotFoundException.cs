namespace Identity.Domain.Exceptions;

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException()
    {
        
    }
    public RoleNotFoundException(string roleName) : base($"{roleName} was not found. Bad Request")
    {
        
    }
}
