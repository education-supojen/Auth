using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Updating.Handler;

public class UpdatePasswordStep3Handler : IRequestHandler<UpdatePasswordStep3Command,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UpdatePasswordStep3Handler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<OneOf<bool,Failure>> Handle(UpdatePasswordStep3Command request, CancellationToken cancellationToken)
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
        var passwordUpdate = await _unitOfWork.PasswordUpdateRepository.GetAsync(email);

        // Processing - 檢查是否有完成前兩步
        if (passwordUpdate == null) return Failures.Update.Null;
        
        // Processing - 是否已經完全確認身份, 可以到填寫資料註冊使用者那一步了?
        var updateTask = passwordUpdate.IfReadyToUpdatePassword(securitystamp);

        // Processing - 如果更新密碼任務失敗, 回傳失敗結果
        if (updateTask.IsT1) return updateTask;
        
        // Processing - 取得使用者資料
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
        if (user == null) return Failures.User.Null;
        
        // Processing - 註冊
        passwordUpdate.Update(user,request.Password);
        
        // Processing - 刪除註冊資料
        await _unitOfWork.PasswordUpdateRepository.DeleteAsync(passwordUpdate);

        // ----------------------------------------------------------------------------------------------------
        // Processing - End Transaction
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.CommitAsync();
        
        // Mission Complete
        return true;
    }
}