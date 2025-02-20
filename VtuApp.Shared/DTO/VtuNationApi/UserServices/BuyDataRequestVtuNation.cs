using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record BuyDataRequestVtuNation
{
    [JsonPropertyName("data_plan")]
    public string DataPlan { get; init; }

    public string Network { get; init; }


    [JsonPropertyName("mobile_number")]
    public string MobileNumber { get; init; }
}
