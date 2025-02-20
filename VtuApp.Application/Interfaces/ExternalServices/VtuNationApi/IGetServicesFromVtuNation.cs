using Refit;
using VtuApp.Shared.DTO.VtuNationApi.UserServices;

namespace VtuApp.Application.Interfaces.ExternalServices.VtuNationApi;

public interface IGetServicesFromVtuNation
{
    // Services

    [Get("/api/services/airtime_networks")]
    Task<ApiResponse<AvailableAirtimeNetworksResponseVtuNation>> GetAvailableAirtimeNetworksAsync();


    [Get("/api/services/data_networks")]
    Task<ApiResponse<AvailableDataNetworksResponseVtuNation>> GetAvailableDataNetworksAsync();


    [Get("/api/services/data/packages/mtn")]
    Task<ApiResponse<AvailableDataPricesVtuNation>> GetMtnDataPricesAsync();


    [Get("/api/services/data/packages/glo")]
    Task<ApiResponse<AvailableDataPricesVtuNation>> GetGloDataPricesAsync();


    [Get("/api/services/data/packages/airtel")]
    Task<ApiResponse<AvailableDataPricesVtuNation>> GetAirtelDataPricesAsync();


    [Get("/api/services/data/packages/9mobile")]
    Task<ApiResponse<AvailableDataPricesVtuNation>> Get9MobileDataPricesAsync();




    // Transactions

    [Post("/api/buy_airtime")]
    Task<ApiResponse<BuyAirtimeResponseVtuNation>> BuyAirtimeVtuNationAsync([Body] BuyAirtimeRequestVtuNation buyAirtimeRequestVtuNation);


    [Post("/api/buy_data")]
    Task<ApiResponse<BuyDataResponseVtuNation>> BuyDataVtuNationAsync([Body] BuyDataRequestVtuNation buyDataRequestVtuNation);

}
