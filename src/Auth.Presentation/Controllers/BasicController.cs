using Auth.Domain.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Presentation.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BasicController : ControllerBase
{
    /// <summary>
    /// Access Token
    /// </summary>
    protected string AccessToken
    {
        get
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            return token;
        }
    }
    
    public IActionResult Handle<T>(OneOf<T, Failure> response)
    {
        // Processing - 
        return response.Match(
            data => Ok(data),
            failure => Problem(
                statusCode: (int) failure.Type,
                title: failure.Code,
                detail: failure.Message)); 
    }
}