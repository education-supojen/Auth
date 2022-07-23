using Auth.Domain.Errors;
using Auth.Presentation.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Presentation.Controllers;


public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        switch (exception)
        {
            case TokenInvalidException:
            {
                var failure = Failures.Token.TokenInvalid;
                return Problem(statusCode:(int)failure.Type,title:failure.Code,detail:failure.Message);
            }
            case TokenExpiredException:
            {
                var failure = Failures.Token.TokenExpire;
                return Problem(statusCode:(int)failure.Type,title:failure.Code,detail:failure.Message);
            }
            default:
                return Problem(statusCode: 500, title: "System.Error", detail: exception!.Message);
        }
    }

    [Route("/error/id")]
    public IActionResult ClientIdError()
    {
        var failure = Failures.Client.IdError;
        return Problem(statusCode:(int)failure.Type,title:failure.Code,detail:failure.Message);
    }
    
    [Route("/error/secret")]
    public IActionResult ClientIdSecret()
    {
        var failure = Failures.Client.SecretError;
        return Problem(statusCode:(int)failure.Type,title:failure.Code,detail:failure.Message);
    }
}