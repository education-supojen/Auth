using Auth.Application.DTO.Feature.Upd;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Updating.Handler;

public class UpdatePasswordStep2Handler : IRequestHandler<UpdatePasswordStep2Command, OneOf<UpdateTokenDto,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UpdatePasswordStep2Handler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    
    public async Task<OneOf<UpdateTokenDto,Failure>> Handle(UpdatePasswordStep2Command request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Begin Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();

        // Processing - 取得更新密碼資料
        var passwordUpdate = await _unitOfWork.PasswordUpdateRepository.GetAsync(request.Email);
        if (passwordUpdate == null) return Failures.Update.Null;
        
        // Processing - 驗證驗證碼
        passwordUpdate.VerifyCode(request.Code);

        // Processing - 處存更新
        await _unitOfWork.PasswordUpdateRepository.UpdateAsync(passwordUpdate);
        
        // ----------------------------------------------------------------------------------------------------
        // End Transaction 
        // ----------------------------------------------------------------------------------------------------
        var commit = await _unitOfWork.CommitAsync();

        // Commit Failure
        if (commit.IsT1) return commit.AsT1;
        
        // Processing - 計算更新用令牌
        var token = _jwtTokenGenerator.GenerateToken(request.Email, passwordUpdate.SecurityStamp);
        
        // Return - 返回令牌
        return new UpdateTokenDto(token);
    }
}