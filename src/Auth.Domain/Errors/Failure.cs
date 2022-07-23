using System.Text.Json.Serialization;

namespace Auth.Domain.Errors;


public record Failure
{
    /// <summary>
    /// 自定義錯誤碼
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string Message { get; }

    
    /// <summary>
    /// 錯誤種類(BadRequest或其他)
    /// </summary>
    public FailureType Type { get; }

    internal Failure(string code, string message, FailureType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    /// <summary>
    /// BadRequest - 400
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Failure BadRequest(string code, string message) => new (code, message, FailureType.BadRequest);

    /// <summary>
    /// UnAuthorized - 401
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Failure UnAuthorized(string code, string message) => new(code, message, FailureType.UnAuthorized);

    /// <summary>
    /// Forbidden - 403
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Failure Forbidden(string code, string message) => new(code, message, FailureType.Forbidden);

    /// <summary>
    /// Conflict - 409
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Failure Conflict(string code, string message) => new(code, message, FailureType.Conflict);

    
    /// <summary>
    /// UnProcessableEntity - 422
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Failure UnProcessableEntity(string code, string message) => new(code, message, FailureType.UnProcessableEntity);
    
}

