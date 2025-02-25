using SharedKernel.Application.Interfaces;

namespace SharedKernel.Application.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

}
