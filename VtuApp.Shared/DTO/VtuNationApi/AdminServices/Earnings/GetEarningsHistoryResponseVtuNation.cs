using System.Text.Json.Serialization;
using VtuApp.Shared.DTO.VtuNationApi.AdminServices.Transaction;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Earnings;

public record GetEarningsHistoryResponseVtuNation
{
    [JsonPropertyName("current_page")]
    public int C1urrentPage { get; init; }

    public List<EarningHistoryDataVtuNation> Data { get; init; }


    [JsonPropertyName("first_page_url")]
    public string FirstPageUrl { get; init; }

    public int From { get; init; }


    [JsonPropertyName("last_page")]
    public int LastPage { get; init; }


    [JsonPropertyName("last_page_url")]
    public string LastPageUrl { get; init; }


    public List<VtuNationLink> Links { get; init; }


    [JsonPropertyName("next_page_url")]
    public string NextPageUrl { get; init; }


    public string Path { get; init; }


    [JsonPropertyName("per_page")]
    public int PerPage { get; init; }


    [JsonPropertyName("prev_page_url")]
    public string PrevPageUrl { get; init; }


    public int To { get; init; }

    public int Total { get; init; }
}
