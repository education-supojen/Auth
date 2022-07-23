using Auth.Domain.Errors;
using Auth.Shared.Profiles;

namespace Auth.Presentation.Contract;

public class ErrorResponse : IMappingFrom<Error>
{
    /// <summary>
    /// 錯誤代碼
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Basic Constructor
    /// </summary>
    /// <param name="message"></param>
    public ErrorResponse(string message) => (Code, Message) = ("n\a", message);

    /// <summary>
    /// General Error
    /// </summary>
    /// <param name="ex"></param>
    public ErrorResponse(Error ex) => (Code, Message) = (ex.Code, ex.Message);
    
    /// <summary>
    /// Bad Request Error
    /// </summary>
    /// <param name="ex"></param>
    public ErrorResponse(BadRequestException ex) => (Code, Message) = (ex.Code, ex.Message);
    
    /// <summary>
    /// Unauthorized Error
    /// </summary>
    /// <param name="ex"></param>
    public ErrorResponse(UnauthorizedException ex) => (Code, Message) = (ex.Code, ex.Message);
    
    /// <summary>
    /// Forbidden Error
    /// </summary>
    /// <param name="ex"></param>
    public ErrorResponse(ForbiddenException ex) => (Code, Message) = (ex.Code, ex.Message);
    
    /// <summary>
    /// Conflict Error
    /// </summary>
    /// <param name="ex"></param>
    public ErrorResponse(ConflictException ex) => (Code, Message) = (ex.Code, ex.Message);
    
    /// <summary>
    /// UnprocessableEntity Error
    /// </summary>
    /// <param name="ex"></param>
    public ErrorResponse(UnprocessableEntityException ex) => (Code, Message) = (ex.Code, ex.Message);
}