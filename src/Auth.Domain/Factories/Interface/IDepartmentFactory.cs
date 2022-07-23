using Auth.Domain.Aggregates;

namespace Auth.Domain.Factories.Interface;

public interface IDepartmentFactory
{
    /// <summary>
    /// 建立預設部門
    /// </summary>
    /// <returns></returns>
    Department Create();
}