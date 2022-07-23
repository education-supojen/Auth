using Auth.Application.Interfaces.Services;
using Auth.Domain.Errors;
using Auth.Domain.Events.Updating;
using MediatR;
using OneOf;

namespace Auth.Application.EventHandler.UpdatePasswordAggregate;

public class ApplyUpdatePasswordEventHandler : IRequestHandler<ApplyUpdatePasswordEvent,OneOf<bool,Failure>>
{
    private readonly IEmailMediator _mediator;

    public ApplyUpdatePasswordEventHandler(IEmailMediator mediator) => _mediator = mediator;

    public async Task<OneOf<bool,Failure>> Handle(ApplyUpdatePasswordEvent notification, CancellationToken cancellationToken)
    {
        // Processing - 寄送更換密碼用驗證碼
        await _mediator.UpdatePasswordAsync(notification.Email, notification.Code);

        // Mission Complete
        return true;
    }
}