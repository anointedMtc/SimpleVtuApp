using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.UserServices;

public record BuyAirtimeAndDataListCollection
{
    public int Id { get; init; }
    public string Receiver { get; init; }
    public string Sender { get; init; }
    public string Description { get; init; }

    [JsonPropertyName("request_id")]
    public string RequestId { get; init; }

    [JsonPropertyName("tran_id")]
    public string TransactionId { get; init; }

    public int Status { get; init; }

    [JsonPropertyName("transaction_type")]
    public string TransactionType { get; init; }

    public string Amount { get; init; }

    [JsonPropertyName("init_bal")]
    public string InitialBalance { get; init; }

    [JsonPropertyName("final_bal")]
    public string FinalBalance { get; init; }

    public string Provider { get; init; }
}
