namespace Identity.Application.Exceptions;

public class CustomNotFoundException : Exception
{
    public CustomNotFoundException(string resourceType, string resourceIdentifier)
        : base($"{resourceType} with id: {resourceIdentifier} doesn't exist")

    {

    }
}
