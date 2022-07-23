namespace Auth.Application.Interfaces.Services;

public interface IIdentityProducer
{
    /// <summary>
    /// 生產一個 ID 出來
    /// </summary>
    /// <returns></returns>
    public long GetId();
}