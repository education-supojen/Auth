using Auth.Application.DTO.Feature.Auth;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Application.Queries.Feature.Auth;
using Auth.Domain.Errors;
using MapsterMapper;
using MediatR;
using OneOf;

namespace Auth.Infrastructure.Queries.Feature.Auth;

public class StaffInformationQueryHandler : IRequestHandler<StaffInformationQuery,OneOf<UserInformationDto,Failure>>
{
    private IUnitOfWork _unitOfWork;
    private IJwtTokenGenerator _jwtTokenGenerator;
    private IMapper _mapper;

    public StaffInformationQueryHandler(IUnitOfWork unitOfWork,IJwtTokenGenerator jwtTokenGenerator,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
    }
    
    public async Task<OneOf<UserInformationDto, Failure>> Handle(StaffInformationQuery request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Begin Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 解讀 Token
        var tokenVariables = _jwtTokenGenerator.ReadSubAndJti(request.Token);

        // Processing - 如果解讀失敗, 返回錯誤結果
        if (tokenVariables.IsT1) return tokenVariables.AsT1;

        // Processing - Sub 轉換成使用者 ID
        var id = long.Parse(tokenVariables.AsT0.sub);

        // Processing - 從 Cache 中取出使用者資訊
        var staff = await _unitOfWork.StaffRepository.CacheGetAsync(id);

        // Processing - 如果使用者為空, 代表無法認證身份
        if (staff == null) return Failures.Token.TokenInvalid;

        // Processing - 如果 securitystamp 不對, 代表無法認證身份
        if (staff.SecurityStamp != tokenVariables.AsT0.jti) return Failures.Token.TokenInvalid;
        
        // ----------------------------------------------------------------------------------------------------
        // End Transaction 
        // ----------------------------------------------------------------------------------------------------
        var commit = await _unitOfWork.CommitAsync();

        // Processing - 如果 Commit 使敗, 返回錯誤結果
        if (commit.IsT1) return commit.AsT1;
        
        // Processing - 回傳使用者資訊
        return _mapper.Map<UserInformationDto>(staff);
    }
}