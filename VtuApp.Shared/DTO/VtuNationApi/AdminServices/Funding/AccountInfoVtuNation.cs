namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Funding;

public record AccountInfoVtuNation
{
    public string AccountName { get; init; }
    public string AccountNumber { get; init; }
    public string BankName { get; init; }
    public string BankCode { get; init; }
    public bool IsCompanyAccount { get; init; }
    public string Note {  get; init; }
}
