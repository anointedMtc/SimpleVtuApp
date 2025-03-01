namespace VtuApp.Shared.DTO;

public record CustomerDto
{
    public Guid CustomerId { get; set; }    
    public Guid ApplicationUserId { get; set; } 
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public decimal VtuBonusBalance { get; set; }
    public decimal TotalBalance { get; set; }
    public int NumberOfStars { get; set; }
    public int TransactionCount { get; set; }
    public TimeSpan TimeLastStarWasAchieved { get; set; } 
    public List<VtuBonusTransferDto> VtuBonusTransfers { get; set; }
    public List<VtuTransactionDto> VtuTransactions { get; set; }

}
