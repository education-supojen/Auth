using Auth.Application.DTO.Feature.Auth;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth.Handler;

public class UserRefreshTokenHandler : IRequestHandler<UserRefreshTokenCommand, OneOf<TokenDto,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public UserRefreshTokenHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }
    
    public async Task<OneOf<TokenDto,Failure>> Handle(UserRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Begin Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 解讀 Token
        var token = _jwtTokenGenerator.ReadSubAndJti(request.AccessToken);

        // Processing - Token 解讀錯誤
        if (token.IsT1) return token.AsT1;

        var userId = long.Parse(token.AsT0.sub);

        // Processing - 取得使用者資料
        var user = await _unitOfWork.UserRepository.GetAsync(userId);
        if (user == null) return Failures.Token.TokenInvalid;
        
        // Processing - 檢查簽名是否正確
        if (user.SecurityStamp != token.AsT0.jti) return Failures.Token.TokenInvalid;
        
        // Processing - 刷新簽名資訊和延長 Refresh 令牌有效期限
        user.RefreshSecurityParams();
        
        // Processing - 設定使用者裝置令牌
        var setDeviceTokenTask = user.SetDeviceToken(request.DeviceType,request.DeviceToken);

        // Processing - 如果設定使用者裝置令牌失敗, 返回失敗結果
        if (setDeviceTokenTask.IsT1) return setDeviceTokenTask.AsT1;
            
        // Processing - 處存變更
        _unitOfWork.UserRepository.Update(user);
        
        // ----------------------------------------------------------------------------------------------------
        // End Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.CommitAsync();

        // Processing - 生產 Token
        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.SecurityStamp);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken(user);

        // Processing - 返回 Token
        return new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}