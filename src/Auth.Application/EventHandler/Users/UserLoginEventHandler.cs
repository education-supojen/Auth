using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Events.Users;
using MediatR;
using OneOf;

namespace Auth.Application.EventHandler.Users;

public class UserLoginEventHandler : IRequestHandler<UserLoginEvent,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UserLoginEventHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OneOf<bool,Failure>> Handle(UserLoginEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRepository.CacheAddAsync(notification.user);
        return true;
    }
}