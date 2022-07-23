using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Events.Updating;
using MediatR;
using OneOf;

namespace Auth.Application.EventHandler.UpdatePasswordAggregate;

public class ReadyToUpdatePasswordEventHandler : IRequestHandler<ReadyToUpdatePasswordEvent, OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHashService _passwordHashService;

    public ReadyToUpdatePasswordEventHandler(IUnitOfWork unitOfWork,IPasswordHashService passwordHashService)
    {
        _unitOfWork = unitOfWork;
        _passwordHashService = passwordHashService;
    }


    public Task<OneOf<bool,Failure>> Handle(ReadyToUpdatePasswordEvent notification, CancellationToken cancellationToken)
    {
        // Processing - 把新的密碼做哈希
        var password = _passwordHashService.Pbkdf2(notification.password);
        // Processing - 更新使用者密碼
        notification.user.EditPassword(password);
        // Processing - 處存更新
        _unitOfWork.UserRepository.Update(notification.user);
        // Mission Complete
        return Task.FromResult<OneOf<bool,Failure>>(true);
    }
}