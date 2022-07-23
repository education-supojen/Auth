using Auth.Application.DTO.Feature.Auth;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;


namespace Auth.Application.Commands.Feature.Auth.Handler;

public class UserLoginHandler : IRequestHandler<UserLoginCommand, OneOf<TokenDto,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public UserLoginHandler(
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
    
    public async Task<OneOf<TokenDto,Failure>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Transaction Begin
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 取得使用者 Domain Model
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
        if (user == null) return Failures.User.Null;
        
        // Processing - 使用者密碼確認
        var passwordValidation = _passwordHashService.PasswordValidation(request.Password, user.Password.HashPassword, user.Password.Salt);
        if (passwordValidation == false) return Failures.User.PasswordWrong;
        
        // Processing - 登入
        user.Login(request.DeviceType,request.DeviceToken);
        
        // Processing - 更新使用者狀態
        _unitOfWork.UserRepository.Update(user);
        
        // ----------------------------------------------------------------------------------------------------
        // Transaction End
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.CommitAsync();
        
        // Processing - 計算 Token
        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id,user.SecurityStamp);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken(user);
        
        // Return - 返回 Token
        return new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
}