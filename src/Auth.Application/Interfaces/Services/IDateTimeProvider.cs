namespace Auth.Application.Interfaces.Services;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}