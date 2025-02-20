namespace VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

public interface ITokenStoreForVtuNation
{
    Task<string> GetVtuNationApiToken();
}
