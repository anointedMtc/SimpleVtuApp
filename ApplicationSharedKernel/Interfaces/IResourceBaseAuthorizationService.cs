namespace SharedKernel.Application.Interfaces;

public interface IResourceBaseAuthorizationService
{
    bool Authorize(string resourceOperation);

}
