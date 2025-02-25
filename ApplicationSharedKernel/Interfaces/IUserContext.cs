using SharedKernel.Application.HelperClasses;

namespace SharedKernel.Application.Interfaces;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
