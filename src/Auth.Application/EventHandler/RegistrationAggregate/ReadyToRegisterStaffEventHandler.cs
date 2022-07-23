using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Events.Registrations;
using Auth.Domain.Factories.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Auth.Application.EventHandler.RegistrationAggregate;

public class ReadyToRegisterStaffEventHandler : IRequestHandler<ReadyToRegisterStaffEvent,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IStaffFactory _factory;
    private readonly ILogger<ReadyToRegisterStaffEventHandler> _logger;

    public ReadyToRegisterStaffEventHandler(
        IUnitOfWork unitOfWork,
        IPasswordHashService passwordHashService,
        IStaffFactory factory,
        ILogger<ReadyToRegisterStaffEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHashService = passwordHashService;
        _factory = factory;
        _logger = logger;
    }
    
    
    public Task<OneOf<bool,Failure>> Handle(ReadyToRegisterStaffEvent notification, CancellationToken cancellationToken)
    {
        // Processing - 建立新註冊的使用者
        var password = _passwordHashService.Pbkdf2(notification.Password);
        var user = _factory.Create(notification.Email, password);
        
        // Processing - 處存更新
        _unitOfWork.StaffRepository.Add(user);

        // Description - 沒有用到任何 await
        return Task.FromResult<OneOf<bool, Failure>>(true);
    }
}