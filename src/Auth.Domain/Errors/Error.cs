using System.Text.Json.Serialization;

namespace Auth.Domain.Errors;


public class Error : Exception
{
    /// <summary>
    /// 自定義錯誤碼
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string Message { get; }

    internal Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
    
    public static BadRequestException BadRequest(string code, string message) => new (code, message);

    public static UnauthorizedException Unauthorized(string code, string message) => new (code, message);

    public static ForbiddenException Forbidden(string code, string message) => new (code, message);

    public static ConflictException Conflict(string code, string message) => new (code, message);

    public static UnprocessableEntityException UnprocessableEntity(string code, string message) => new (code, message);
}

/// <summary>
/// General Error
/// </summary>
public class BadRequestException : Error
{
    internal BadRequestException(string code, string message) : base(code, message)
    { }
}

/// <summary>
/// 使用者身份未被認證
/// </summary>
public class UnauthorizedException : Error 
{
    internal UnauthorizedException(string code, string message) : base(code, message)
    { }
}

/// <summary>
/// 使用者權限不夠
/// </summary>
public class ForbiddenException : Error
{
    internal ForbiddenException(string code, string message) : base(code, message)
    { }
}

/// <summary>
/// 會使伺服器(會是資料)產生衝突
/// </summary>
public class ConflictException : Error
{
    internal ConflictException(string code, string message) : base(code, message)
    { }
}

/// <summary>
/// 資料格式不對
/// </summary>
public class UnprocessableEntityException : Error
{
    internal UnprocessableEntityException(string code, string message) : base(code, message)
    { }
}