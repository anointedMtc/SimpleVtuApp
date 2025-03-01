namespace VtuApp.Shared.DTO;

public record CustomerShortResponseDto
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
}
