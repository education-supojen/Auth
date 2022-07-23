using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Events.Registrations;
using Auth.Domain.Factories.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Auth.Application.EventHandler.RegistrationAggregate;

public class ReadyToRegisterUserEventHandler : IRequestHandler<ReadyToRegisterUserEvent,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IUserFactory _userFactory;
    private readonly ILogger<ReadyToRegisterUserEventHandler> _logger;

    public ReadyToRegisterUserEventHandler(
        IUnitOfWork unitOfWork,
        IPasswordHashService passwordHashService,
        IUserFactory userFactory,
        ILogger<ReadyToRegisterUserEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHashService = passwordHashService;
        _userFactory = userFactory;
        _logger = logger;
    }
    
    
    public Task<OneOf<bool,Failure>> Handle(ReadyToRegisterUserEvent notification, CancellationToken cancellationToken)
    {
        // Processing - 建立新註冊的使用者
        var password = _passwordHashService.Pbkdf2(notification.Password);
        var user = _userFactory.Create(email: notification.Email, password: password);
        
        // Processing - 處存更新
        _unitOfWork.UserRepository.Add(user);

        // Mission Complete
        return Task.FromResult<OneOf<bool,Failure>>(true);
    }
}