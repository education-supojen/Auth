using Auth.Application.DTO;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;

namespace Auth.Application.Commands.FeatureAuth.Handler;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, TokenDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAesService _aesService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public RefreshTokenHandler(
        IUnitOfWork unitOfWork,
        IAesService aesService,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _aesService = aesService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }
    
    public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Processing - 從 jwt token 讀取使用者 ID 和簽名
        var (sub, jti) = _jwtTokenGenerator.ReadSubAndJti(request.AccessToken);
        var userId = long.Parse(_aesService.DecryptCBC(sub));
        var userSecurityStamp = _aesService.DecryptCBC(jti);

        // Processing - 取得使用者資料
        var user = await _unitOfWork.UserRepository.GetAsync(userId);

        // Processing - 檢查簽名是否正確
        if (user.SecurityStamp != userSecurityStamp) throw Errors.Token.TokenInvalid;
        
        // Processing - 刷新簽名資訊和延長 Refresh 令牌有效期限
        user.RefreshSecurityParams();
        
        // Processing - 設定使用者裝置令牌
        user.SetDeviceToken(request.DeviceType,request.DeviceToken);

        // Processing - 處存變更
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();

        // Processing - 
        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id, user.SecurityStamp);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken(user);

        // Processing - 
        return new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}