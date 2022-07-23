namespace Auth.Domain.Enums;

public enum Permission
{
    /// <summary>
    /// 公司所有者
    /// </summary>
    admin = 0,
    
    /// <summary>
    /// 後台權限
    /// </summary>
    backstage = 1,
    
    /// <summary>
    /// 部門主管
    /// </summary>
    manager = 2,
    
    /// <summary>
    /// 僱員
    /// </summary>
    employee = 3
}