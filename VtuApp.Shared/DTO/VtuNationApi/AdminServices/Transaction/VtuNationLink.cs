namespace VtuApp.Shared.DTO.VtuNationApi.AdminServices.Transaction;

public record VtuNationLink
{
    public string Url { get; init; }

    public string Label { get; init; }

    public bool Active { get; init; }
}
