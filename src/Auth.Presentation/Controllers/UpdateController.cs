using Auth.Application.Commands.Feature.Updating;
using Auth.Application.DTO.Feature.Upd;
using Auth.Presentation.Contract.Feature.Upt;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Auth.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UpdateController : BasicController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UpdateController(IMediator mediator,IMapper mapper) => (_mediator,_mapper) = (mediator,mapper);
    
    /// <summary>
    /// 更改密碼 - 第一步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Password/Step/1")]
    [OpenApiTags("Feature - Update (更新類)")]
    public async Task<IActionResult> PasswordUpdateStep1Async([FromBody]UpdatePasswordStep1Request request)
    {
        // Processing - 
        var command = _mapper.Map<UpdatePasswordStep1Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 更改密碼 - 第二步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Password/Step/2")]
    [OpenApiTags("Feature - Update (更新類)")]
    [ProducesResponseType(typeof(UpdateTokenDto),200)]
    public async Task<IActionResult> PasswordUpdateStep2Async([FromBody]UpdatePasswordStep2Request request)
    {
        // Processing - 
        var command = _mapper.Map<UpdatePasswordStep2Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 更改密碼 - 第三步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Password/Step/3")]
    [OpenApiTags("Feature - Update (更新類)")]
    public async Task<IActionResult> PasswordUpdateStep3Async([FromBody]UpdatePasswordStep3Request request)
    {
        // Processing - 
        var command = _mapper.Map<UpdatePasswordStep3Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
}