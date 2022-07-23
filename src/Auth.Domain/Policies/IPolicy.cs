namespace Auth.Domain.Policies;

/// <summary>
/// Policy Pattern
/// https://www.codeproject.com/Tips/1175911/Domain-Policy-for-Domain-Driven-Design
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPolicy<T>
{
    /// <summary>
    /// 是否符合條件, 若符合條件 GenerateItems 給出的 items 都可以拿來運用
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool IsApplicable(IPolicyData data);
    
    /// <summary>
    /// 產生出符合條件得 Items
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    IEnumerable<T> GenerateItems(IPolicyData data);
}