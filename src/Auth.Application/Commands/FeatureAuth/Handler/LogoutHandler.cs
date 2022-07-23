using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using MediatR;

namespace Auth.Application.Commands.FeatureAuth.Handler;

public class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAesService _aesService;

    public LogoutHandler(IUnitOfWork unitOfWork, IAesService aesService)
    {
        _unitOfWork = unitOfWork;
        _aesService = aesService;
    }
    
    
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Processing - 解碼 cipher 取得使用者 ID
        var id = long.Parse(_aesService.DecryptCBC(request.token));

        // Processing - 讀取使用者
        var user = await _unitOfWork.UserRepository.GetAsync(id);
        
        // Processing - 登出
        user.Logout();

        // Processing - 處存更新
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UserRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
        
        return Unit.Value;
    }
}