using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Events.Users;
using MediatR;
using OneOf;

namespace Auth.Application.EventHandler.Users;

public class UserLogoutEventHandler : IRequestHandler<UserLogoutEvent,OneOf<bool,Failure>>
{
    private IUnitOfWork _unitOfWork;

    public UserLogoutEventHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    
    public async Task<OneOf<bool,Failure>> Handle(UserLogoutEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRepository.CacheDeleteAsync(notification.user);
        return true;
    }
}