using Auth.Application.DTO.Feature.Auth;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth.Handler;

public class StaffRefreshTokenHandler : IRequestHandler<StaffRefreshTokenCommand,OneOf<TokenDto,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public StaffRefreshTokenHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }
    
    public async Task<OneOf<TokenDto,Failure>> Handle(StaffRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Begin Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 從 jwt token 讀取使用者 ID 和簽名
        var token = _jwtTokenGenerator.ReadSubAndJti(request.AccessToken);

        // Processing - Token 解讀錯誤
        if (token.IsT1) return token.AsT1;
        
        var staffId = long.Parse(token.AsT0.sub);

        // Processing - 取得使用者資料
        var staff = await _unitOfWork.StaffRepository.GetAsync(staffId);
        if (staff == null) return Failures.Token.TokenInvalid;
        
        // Processing - 檢查簽名是否正確
        if (staff.SecurityStamp != token.AsT0.jti) return Failures.Token.TokenInvalid;
        
        // Processing - 刷新簽名資訊和延長 Refresh 令牌有效期限
        staff.RefreshSecurityParams();
        
        // Processing - 設定使用者裝置令牌
        var task = staff.SetDeviceToken(request.DeviceType,request.DeviceToken);

        // Processing - 如果設定失敗, 翻回錯誤結果
        if (task.IsT0) return task.AsT1;
        
        // Processing - 處存變更
        _unitOfWork.StaffRepository.Update(staff);
        
        // ----------------------------------------------------------------------------------------------------
        // End Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.CommitAsync();

        // Processing - 生產 Token
        var accessToken = _jwtTokenGenerator.GenerateToken(staff.Id, staff.SecurityStamp);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken(staff);

        // Processing - 返回 Token
        return new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}