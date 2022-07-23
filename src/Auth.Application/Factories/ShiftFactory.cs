using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Interface;

namespace Auth.Application.Factories;

public class ShiftFactory : IShiftFactory
{
    /// <summary>
    /// 建立標準輪班表
    /// </summary>
    /// <returns></returns>
    public Shift Create()
    {
        return new Shift("標準輪班", new TimeOnly(09, 00), new TimeOnly(18, 00), new TimeOnly(12, 00), new TimeOnly(13, 00), 30);
    }
}