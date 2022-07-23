namespace Auth.Application.Interfaces.Services;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}