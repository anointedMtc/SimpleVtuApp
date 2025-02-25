namespace SharedKernel.Application.Interfaces;

public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}
