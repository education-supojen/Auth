using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Interface;

namespace Auth.Domain.Factories;

public class ScheduleFactory : IScheduleFactory
{
    /// <summary>
    /// 建立標準工作表
    /// </summary>
    /// <returns></returns>
    public Schedule Create()
    {
        return new Schedule("標準工作表", new DayOfWeek[]
        {
            DayOfWeek.Wednesday, DayOfWeek.Sunday
        });
    }
}