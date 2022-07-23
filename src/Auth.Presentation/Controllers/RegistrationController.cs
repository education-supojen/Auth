using Auth.Application.Commands.Feature.Register;
using Auth.Application.DTO.Feature.Reg;
using Auth.Presentation.Contract.Feature.Reg;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Auth.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class RegistrationController : BasicController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public RegistrationController(IMediator mediator, IMapper mapper) => (_mediator,_mapper) = (mediator,mapper);

    /// <summary>
    /// 後台人員註冊 - 第1步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Staff/Step/1")]
    [OpenApiTags("Feature - 註冊類")]
    public async Task<IActionResult> StaffStep1sync([FromBody]RegistrationStep1Request request)
    {
        // Processing - 
        var command = _mapper.Map<StaffRegistrationStep1Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 後台人員註冊 - 第2步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Staff/Step/2")]
    [OpenApiTags("Feature - 註冊類")]
    public async Task<IActionResult> StaffStep2sync([FromBody]RegistrationStep2Request request)
    {
        // Processing - 
        var command = _mapper.Map<StaffRegistrationStep2Command>(request);
        // Processing -
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 後台人員註冊 - 第3步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Staff/Step/3")]
    [OpenApiTags("Feature - 註冊類")]
    public async Task<IActionResult> StaffStep3sync([FromBody]RegistrationStep3Request request)
    {
        // Processing - 
        var command = _mapper.Map<StaffRegistrationStep3Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }

    /// <summary>
    /// 會員註冊 - 第1步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("User/Step/1")]
    [OpenApiTags("Feature - 註冊類")]
    public async Task<IActionResult> UserStep1Async([FromBody]RegistrationStep1Request request)
    {
        // Processing - 
        var command = _mapper.Map<UserRegistrationStep1Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 會員註冊 - 第2步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("User/Step/2")]
    [OpenApiTags("Feature - 註冊類")]
    [ProducesResponseType(typeof(RegistrationTokenDto),200)]
    public async Task<IActionResult> UserStep2Async([FromBody]RegistrationStep2Request request)
    {
        // Processing - 
        var command = _mapper.Map<UserRegistrationStep2Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
    
    /// <summary>
    /// 會員註冊 - 第3步
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("User/Step/3")]
    [OpenApiTags("Feature - 註冊類")]
    public async Task<IActionResult> UserStep3Async([FromBody]RegistrationStep3Request request)
    {
        // Processing - 
        var command = _mapper.Map<UserRegistrationStep3Command>(request);
        // Processing - 
        var response = await _mediator.Send(command);
        // Processing - 
        return Handle(response);
    }
}