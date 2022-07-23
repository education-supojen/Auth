using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Register.Handler;

public class UserRegistrationStep3Handler : IRequestHandler<UserRegistrationStep3Command,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserRegistrationStep3Handler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<OneOf<bool,Failure>> Handle(UserRegistrationStep3Command request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Processing - Begin Transaction        
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 解讀註冊令牌
        var jwtToken = _jwtTokenGenerator.ReadSubAndJti(request.Token);

        // Processing - JWT Token 解讀錯誤, 返回錯誤結果
        if (jwtToken.IsT1) return jwtToken.AsT1;

        // Variable - 郵箱
        var email = jwtToken.AsT0.sub;
        
        // Variable - securitystamp
        var securitystamp = jwtToken.AsT0.jti;

        // Processing - 取得註冊中使用者的 Model
        var registration = await _unitOfWork.RegistrationRepository.GetAsync(email);

        // Processing - 檢查是否有完成前兩步
        if (registration == null) return Failures.Registration.Null;
        
        // Processing - 是否已經完全確認身份, 可以到填寫資料註冊使用者那一步了?
        var task = registration.IfReadyToRegister(securitystamp);

        // Processing - 如果還沒準備好, 返回錯誤結果
        if (task.IsT1) return task.AsT1;
        
        // Processing - 註冊
        registration.RegisterForUser(request.Password);
        
        // Processing - 刪除註冊資料
        await _unitOfWork.RegistrationRepository.DeleteAsync(registration);

        // ----------------------------------------------------------------------------------------------------
        // Processing - End Transaction
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.CommitAsync();

        // Mission Complete
        return true;
    }
}