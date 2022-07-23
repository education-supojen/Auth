using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Presentation.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BasicController : ControllerBase
{
    /// <summary>
    /// Access Token
    /// </summary>
    protected string Token
    {
        get
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            return token;
        }
    }


    protected string UserId
    {
        get
        {
            return User.Identity.Name;
        }
    }
}