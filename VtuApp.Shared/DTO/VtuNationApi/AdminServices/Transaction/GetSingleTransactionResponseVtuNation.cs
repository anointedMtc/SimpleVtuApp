using System.Text.Json.Serialization;

namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Transaction;

public record GetSingleTransactionResponseVtuNation
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

    public decimal Amount { get; init; }

    [JsonPropertyName("init_bal")]
    public decimal InitialBalance { get; init; }

    [JsonPropertyName("final_bal")]
    public decimal FinalBalance { get; init; }

    public string Provider { get; init; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; }
}
