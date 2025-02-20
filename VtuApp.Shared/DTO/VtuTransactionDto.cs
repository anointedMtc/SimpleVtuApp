namespace VtuApp.Shared.DTO;

public class VtuTransactionDto
{
    public Guid CustomerId { get; set; }
    public string TypeOfTransaction { get; set; }
    public string NetWorkProvider { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Status { get; set; }

}
