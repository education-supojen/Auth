using Auth.Application.DTO;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;

namespace Auth.Application.Commands.FeatureAuth.Handler;

public class LoginHandler : IRequestHandler<LoginCommand, TokenDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public LoginHandler(
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
    
    public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Processing - 取得使用者 domain model
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);

        // Processing - 使用者密碼確認
        var passwordValidation= _passwordHashService.PasswordValidation(request.Password, user.PasswordHash, user.Salt);
        if (passwordValidation == false) throw Errors.User.PasswordWrong;
        
        // Processing - 登入
        user.Login(request.DeviceType,request.DeviceToken ?? string.Empty);
        
        // Processing - 更新使用者狀態
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();

        // Processing - 計算 token
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