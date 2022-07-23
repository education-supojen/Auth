using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Auth.Handler;

public class UserLogoutHandler : IRequestHandler<UserLogoutCommand,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAesService _aesService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserLogoutHandler(IUnitOfWork unitOfWork, IAesService aesService,IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _aesService = aesService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    
    public async Task<OneOf<bool,Failure>> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Transaction Begin
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 解讀 Token
        var token = _jwtTokenGenerator.ReadSubAndJti(request.token);

        // Processing - Token 解讀錯誤
        if (token.IsT1) return token.AsT1;
        
        // Processing - Sub 轉換成使用者 ID
        var id = long.Parse(token.AsT0.sub);

        // Processing - 讀取使用者
        var user = await _unitOfWork.UserRepository.GetAsync(id);
        if (user == null) return Failures.User.Null;
        
        // Processing - 登出
        user.Logout();

        // Processing - 處存更新
        _unitOfWork.UserRepository.Update(user);

        // Processing - 刪除快取
        await _unitOfWork.UserRepository.CacheDeleteAsync(user);
        
        // ----------------------------------------------------------------------------------------------------
        // Transaction End
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.CommitAsync();
        
        // Mission Complete
        return true;
    }
}