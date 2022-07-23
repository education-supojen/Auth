using Auth.Domain.Aggregates;

namespace Auth.Domain.Factories.Interface;

public interface IShiftFactory
{
    Shift Create();
}