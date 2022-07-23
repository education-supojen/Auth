using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Events.Staffs;
using MediatR;
using OneOf;

namespace Auth.Application.EventHandler.Staffs;

public class StaffLogoutEventHandler : IRequestHandler<StaffLogoutEvent,OneOf<bool,Failure>>
{
    private IUnitOfWork _unitOfWork;

    public StaffLogoutEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<OneOf<bool, Failure>> Handle(StaffLogoutEvent request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StaffRepository.CacheDeleteAsync(request.staff);
        return true;
    }
}