using Auth.Application.Commands;
using Auth.Application.Commands.FeatureAuth;
using Auth.Application.Commands.FeatureRegistration;
using Auth.Application.DTO;
using Auth.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Auth.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BasicController
{
    private readonly IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region 註冊類
    
    /// <summary>
    /// 註冊 - 第一步
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("Registration/Step/1")]
    [OpenApiTags("註冊類")]
    [AllowAnonymous]
    public async Task<IActionResult> RegistrationStep1Async([FromBody]RegistrationStep1Command command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// 註冊 - 第二步
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("Registration/Step/2")]
    [OpenApiTags("註冊類")]
    [AllowAnonymous]
    public async Task<IActionResult> RegistrationStep2Async([FromBody]RegistrationStep2Command command)
    {
        await _mediator.Send(command);
        return Ok();
    }
    
    /// <summary>
    /// 註冊 - 第三步
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("Registration/Step/3")]
    [OpenApiTags("註冊類")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenDto),200)]
    public async Task<IActionResult> RegistrationStep3Async([FromBody]RegistrationStep3Command command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }
    
    #endregion
    
    
    
    /// <summary>
    /// 登入 - 郵件號碼
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("Login/Email")]
    [AllowAnonymous]
    [OpenApiTag("登入(出)類")]
    public async Task<ActionResult> EmailLoginAsync([FromBody]LoginCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }
    
    /// <summary>
    /// 登出
    /// </summary>
    /// <returns></returns>
    [HttpPost("Logout")]
    [OpenApiTag("登入(出)類")]
    public async Task<ActionResult> LogoutAsync()
    {
        await _mediator.Send(new LogoutCommand(Token));
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("Refresh")]
    [AllowAnonymous]
    [OpenApiTag("登入(出)類")]
    public async Task<ActionResult> RefreshTokenAsync([FromBody]RefreshTokenCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(token);
    }

    /// <summary>
    /// 取得 - 使用者資訊
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    [HttpGet("Information")]
    [OpenApiTag("登入(出)類")]
    public async Task<ActionResult> MemberInformationAsync()
    {
        var userInformation= await _mediator.Send(new UserInformationQuery(Token));
        return Ok(userInformation);
    }
}