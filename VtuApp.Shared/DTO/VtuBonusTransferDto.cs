using SharedKernel.Common.Constants;

namespace VtuApp.Shared.DTO;

public record VtuBonusTransferDto
{
    public Guid Id { get; set; }
    public decimal AmountTransfered { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal FinalBalance { get; set; }
    public TransferDirection TransferDirection { get; set; }
    public string ReasonWhy { get; set; }
}
