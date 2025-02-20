using ApplicationSharedKernel.HelperClasses;

namespace ApplicationSharedKernel.Interfaces;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
