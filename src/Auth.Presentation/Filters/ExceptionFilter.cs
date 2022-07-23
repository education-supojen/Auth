using System.Text;
using System.Text.Json;
using Auth.Domain.Errors;
using Auth.Presentation.Contract;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Auth.WebApi.Filters;

public class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }


    public async Task OnExceptionAsync(ExceptionContext context)
    {
        // Processing - 從exception 中產生要回傳給使用者的物件
        var response = GetApiResponse(context);

        // Processing - 設定 Response 的內容
        
        _logger.LogError($"\n\n{JsonSerializer.Serialize(response)}\n\n");
        
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions(){ PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

        // Processing - 通知系統 exception 已經被處理過了
        context.ExceptionHandled = true;
    }


    /// <summary>
    /// 從 Exception 中產生 Api Response(回傳給 API 使用者的資訊)
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private ErrorResponse? GetApiResponse(ExceptionContext context)
    {
        var exception = context.Exception;   // 被拋出的 Exception 


        switch (exception)
        {
            // Description: 
            case BadRequestException ex:
                _logger.LogError("BadRequest: {content}",new { ex.Message, ex.Code});
                context.HttpContext.Response.StatusCode = (int)ErrorType.BadRequest;
                return new ErrorResponse(ex);
            
            // Description: 
            case UnauthorizedException ex:
                _logger.LogError("Unauthorized: {content}",new { ex.Message, ex.Code});
                context.HttpContext.Response.StatusCode = (int)ErrorType.Unauthorized;
                return new ErrorResponse(ex);
        
            // Description: 
            case ForbiddenException _:
                _logger.LogError("Forbidden");
                context.HttpContext.Response.StatusCode = (int)ErrorType.Forbidden;
                return null;

            // Description:
            case ConflictException _:
                _logger.LogError("Conflict");
                context.HttpContext.Response.StatusCode = (int)ErrorType.Conflict;
                return null;
        
            // Description:
            case UnprocessableEntityException ex:
                _logger.LogError("UnprocessableEntity: {content}",new { ex.Message, ex.Code});
                context.HttpContext.Response.StatusCode = (int)ErrorType.UnprocessableEntity;
                return new ErrorResponse(ex);
        
            // Description: 
            default:
                _logger.LogCritical(exception.ToString());
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new ErrorResponse(exception.Message);
        }
    }
}