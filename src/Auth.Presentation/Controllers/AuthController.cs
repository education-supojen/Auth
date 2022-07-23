using System.Reflection.Metadata;
using System.Text.Json;
using Auth.Application.Commands;
using Auth.Application.Commands.Feature.Auth;
using Auth.Application.DTO;
using Auth.Application.DTO.Feature.Auth;
using Auth.Application.Queries;
using Auth.Application.Queries.Feature.Auth;
using Auth.Domain.Errors;
using Auth.Presentation.Common;
using Auth.Presentation.Contract.Feature.Auth;
using MapsterMapper;
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
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator,IMapper mapper) => (_mediator,_mapper) = (mediator,mapper);

    /// <summary>
    /// 網站人員 - 登入
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Staff/Login")]
    [AllowAnonymous]
    [OpenApiTag("Feature - Authentication (認證類)")]
    public async Task<IActionResult> StaffEmailLoginAsync([FromBody]LoginRequest request)
    {
        // Processing - 
        var command = _mapper.Map<StaffLoginCommand>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    
    /// <summary>
    /// 使用者 - 登入
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("User/Login")]
    [AllowAnonymous]
    [OpenApiTag("Feature - Authentication (認證類)")]
    public async Task<IActionResult> UserEmailLoginAsync([FromBody]LoginRequest request)
    {
        // Processing - 
        var command = _mapper.Map<UserLoginCommand>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }

    /// <summary>
    /// 網站人員 - 登出
    /// </summary>
    /// <returns></returns>
    [HttpPost("Staff/Logout")]
    [OpenApiTag("Feature - Authentication (認證類)")]
    public async Task<IActionResult> StaffLogoutAsync()
    {
        // Processing- 
        var response = await _mediator.Send(new StaffLogoutCommand(AccessToken));
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 使用者 - 登出
    /// </summary>
    /// <returns></returns>
    [HttpPost("User/Logout")]
    [OpenApiTag("Feature - Authentication (認證類)")]
    public async Task<IActionResult> UserLogoutAsync()
    {
        // Processing - 
        var response = await _mediator.Send(new UserLogoutCommand(AccessToken));
        // Processing - 
        return Handle(response);
    }

    /// <summary>
    /// 網站人員 - RefreshToekn
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Staff/Refresh")]
    [AllowAnonymous]
    [OpenApiTag("Feature - Authentication (認證類)")]
    public async Task<IActionResult> StaffRefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        // Processing - 
        var command = _mapper.Map<StaffRefreshTokenCommand>(request);
        // Processing - 
        command.AccessToken = AccessToken;
        // Processing -
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 使用者 - Refresh Token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("User/Refresh")]
    [AllowAnonymous]
    [OpenApiTag("Feature - Authentication (認證類)")]
    public async Task<IActionResult> UserRefreshTokenAsync([FromBody]RefreshTokenRequest request)
    {
        // Processing - 
        var command = _mapper.Map<UserRefreshTokenCommand>(request);
        // Processing - 
        command.AccessToken = AccessToken;
        // Processing -
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }

    /// <summary>
    /// 取得 - 網站人員資訊
    /// </summary>
    /// <returns></returns>
    [HttpGet("Staff/Information")]
    [OpenApiTag("Feature - Authentication (認證類)")]
    [ProducesResponseType(typeof(UserInformationDto),200)]
    public async Task<IActionResult> StaffInformationAsync()
    {
        // Processing - 
        var query = new StaffInformationQuery(AccessToken);
        // Processing - 
        var response = await _mediator.Send(query);
        // Processing - 
        return Handle(response);
    }
    
    
    /// <summary>
    /// 取得 - 使用者資訊
    /// </summary>
    /// <returns></returns>
    [HttpGet("User/Information")]
    [OpenApiTag("Feature - Authentication (認證類)")]
    [ProducesResponseType(typeof(UserInformationDto),200)]
    public async Task<IActionResult> UserInformationAsync()
    {
        // Processing - 
        var query = new UserInformationQuery(AccessToken);
        // Processing - 
        var response = await _mediator.Send(query);
        // Processing - 
        return Handle(response);
    }
}