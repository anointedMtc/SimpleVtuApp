namespace VtuApp.Application.Exceptions;

public class CustomerAlreadyExistsException(string email) : Exception($"Customer with email: '{email}' already exists.")
{

}
