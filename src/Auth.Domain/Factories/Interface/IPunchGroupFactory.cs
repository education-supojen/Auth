using Auth.Domain.Aggregates;
using Auth.Domain.ValueObjects;

namespace Auth.Domain.Factories.Interface;

public interface IPunchGroupFactory
{
    /// <summary>
    /// 建立表準打卡群組
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    PunchGroup Create(Location location);
}