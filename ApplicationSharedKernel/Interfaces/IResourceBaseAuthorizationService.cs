namespace ApplicationSharedKernel.Interfaces;

public interface IResourceBaseAuthorizationService
{
    bool Authorize(string resourceOperation);

}
