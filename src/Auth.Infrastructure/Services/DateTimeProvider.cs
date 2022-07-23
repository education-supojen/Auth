using Auth.Application.Interfaces.Services;

namespace Auth.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}