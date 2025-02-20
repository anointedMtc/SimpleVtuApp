namespace ApplicationSharedKernel.Interfaces;

public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}
