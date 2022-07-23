using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Interface;
using Auth.Domain.ValueObjects;

namespace Auth.Application.Factories;

public class PunchGroupFactory : IPunchGroupFactory
{
    /// <summary>
    /// 建立表準打卡群組
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public PunchGroup Create(Location location)
    {
        return new PunchGroup("標準打卡群組", location, 100);
    }
}