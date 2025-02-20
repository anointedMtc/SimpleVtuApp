using ApplicationSharedKernel.Interfaces;

namespace ApplicationSharedKernel.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

}
