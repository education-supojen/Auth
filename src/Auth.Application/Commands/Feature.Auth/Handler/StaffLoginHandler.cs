using Auth.Application.DTO.Feature.Auth;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth.Handler;

public class StaffLoginHandler : IRequestHandler<StaffLoginCommand, OneOf<TokenDto,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public StaffLoginHandler(
        IUnitOfWork unitOfWork,
        IPasswordHashService passwordHashService,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHashService = passwordHashService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }
    
    public async Task<OneOf<TokenDto,Failure>> Handle(StaffLoginCommand request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Transaction Begin
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 取得使用者 Domain Model
        var staff = await _unitOfWork.StaffRepository.GetStaffByEmailAsync(request.Email);
        if (staff == null) return Failures.User.Null;
        
        // Processing - 使用者密碼確認
        var passwordValidation = _passwordHashService.PasswordValidation(request.Password, staff.Password.HashPassword, staff.Password.Salt);
        if (passwordValidation == false) return Failures.User.PasswordWrong;
        
        // Processing - 登入
        staff.Login(request.DeviceType,request.DeviceToken);
        
        // Processing - 更新使用者狀態
        _unitOfWork.StaffRepository.Update(staff);
        
        // ----------------------------------------------------------------------------------------------------
        // Transaction End
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.CommitAsync();
        
        // Processing - 計算 Token
        var accessToken = _jwtTokenGenerator.GenerateToken(staff.Id,staff.SecurityStamp);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken(staff);
        
        // Return - 返回 Token
        return new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
}