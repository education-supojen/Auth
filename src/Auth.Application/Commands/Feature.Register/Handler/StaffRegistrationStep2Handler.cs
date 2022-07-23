using Auth.Application.DTO.Feature.Reg;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Enums;
using Auth.Domain.Errors;
using Auth.Domain.Repositories;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Register.Handler;

public class StaffRegistrationStep2Handler : IRequestHandler<StaffRegistrationStep2Command, OneOf<RegistrationTokenDto,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public StaffRegistrationStep2Handler (IUnitOfWork unitOfWork,IRegistrationRepository registrationRepository,IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _registrationRepository = registrationRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<OneOf<RegistrationTokenDto,Failure>> Handle(StaffRegistrationStep2Command request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Begin Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 取得註冊資料
        var registration = await _registrationRepository.GetAsync(request.Email);
        
        // Processing - 如果找不到註冊資料, 代表還沒有申請
        if (registration == null) return Failures.Registration.Null;
        
        // Processing - 驗證驗證碼
        var verificationStatus = registration.VerifyCode(request.Code);
        
        // Processing - 處存更新
        await _registrationRepository.UpdateAsync(registration);
        
        // Processing - 根據驗正錯誤結果, 做出相對應的對策
        switch (verificationStatus)
        {
            case VerificationStatus.Fail:
                return Failures.Registration.CodeUnCorrect;
            case VerificationStatus.HaveNoChance:
                return Failures.Registration.NoChance;
        }
        
        // ----------------------------------------------------------------------------------------------------
        // End Transaction 
        // ----------------------------------------------------------------------------------------------------
        var commit = await _unitOfWork.CommitAsync();

        // Commit Failure
        if (commit.IsT1) return commit.AsT1;
        
        // Processing - 產生註冊令牌
        var token = _jwtTokenGenerator.GenerateToken(request.Email, registration.SecurityStamp);
        
        // Return - 返回令牌
        return new RegistrationTokenDto(token);
    }
}