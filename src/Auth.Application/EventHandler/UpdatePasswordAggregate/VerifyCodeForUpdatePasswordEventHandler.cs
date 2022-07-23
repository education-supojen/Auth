using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Enums;
using Auth.Domain.Errors;
using Auth.Domain.Events.Updating;
using MediatR;
using OneOf;

namespace Auth.Application.EventHandler.UpdatePasswordAggregate;

public class VerifyCodeForUpdatePasswordEventHandler : IRequestHandler<VerifyCodeForUpdatePasswordEvent, OneOf<bool, Failure>>
{
    private readonly IUnitOfWork _unitOfWork;

    public VerifyCodeForUpdatePasswordEventHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OneOf<bool, Failure>> Handle(VerifyCodeForUpdatePasswordEvent notification, CancellationToken cancellationToken)
    {
        // Processing - 處存更新(密碼更新資料)
        await _unitOfWork.PasswordUpdateRepository.UpdateAsync(notification.PasswordUpdate);
        
        // Processing - 若有錯誤, 拋出錯誤
        switch (notification.status)
        {
            case VerificationStatus.Fail:
                return Failures.Registration.CodeUnCorrect;
            case VerificationStatus.HaveNoChance:
                return Failures.Registration.NoChance;
        }

        // Mission Complete
        return true;
    }
}