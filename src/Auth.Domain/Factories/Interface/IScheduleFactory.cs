using Auth.Domain.Aggregates;

namespace Auth.Domain.Factories.Interface;

public interface IScheduleFactory
{
    /// <summary>
    /// 建立標準工作表
    /// </summary>
    /// <returns></returns>
    public Schedule Create();
}