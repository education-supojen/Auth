namespace Auth.Domain.Errors;

public enum FailureType
{
    /// <summary>
    /// General Error
    /// </summary>
    BadRequest = 400,
    
    /// <summary>
    /// 使用者身份未被認證
    /// </summary>
    UnAuthorized = 401,
    
    /// <summary>
    /// 使用者權限不夠
    /// </summary>
    Forbidden = 403,
    
    /// <summary>
    /// 會使伺服器(會是資料)產生衝突
    /// </summary>
    Conflict = 409,
    
    /// <summary>
    /// 資料格式不對
    /// </summary>
    UnProcessableEntity = 422,
}