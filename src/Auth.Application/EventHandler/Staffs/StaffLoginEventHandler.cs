using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Events.Staffs;
using MediatR;
using OneOf;

namespace Auth.Application.EventHandler.Staffs;

public class StaffLoginEventHandler : IRequestHandler<StaffLoginEvent,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;

    public StaffLoginEventHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OneOf<bool,Failure>> Handle(StaffLoginEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.StaffRepository.CacheAddAsync(notification.staff);
        return true;
    }
}