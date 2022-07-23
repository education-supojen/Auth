using Auth.Application.DTO;
using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Errors;
using Auth.Domain.Factories.Interface;
using Auth.Domain.Repositories;
using Auth.Domain.ValueObjects;
using MediatR;

namespace Auth.Application.Commands.FeatureRegistration.Handler;

public class RegistrationStep3Handler : IRequestHandler<RegistrationStep3Command, TokenDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IIdentityProducer _identityProducer;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUserFactory _userFactory;
    private readonly ICompanyFactory _companyFactory;

    public RegistrationStep3Handler(
        IUnitOfWork unitOfWork,
        IRegistrationRepository registrationRepository,
        IPasswordHashService passwordHashService,
        IIdentityProducer identityProducer,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUserFactory userFactory,
        ICompanyFactory companyFactory)
    {
        _unitOfWork = unitOfWork;
        _registrationRepository = registrationRepository;
        _passwordHashService = passwordHashService;
        _identityProducer = identityProducer;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _userFactory = userFactory;
        _companyFactory = companyFactory;
    }
    
    public async Task<TokenDto> Handle(RegistrationStep3Command request, CancellationToken cancellationToken)
    {
        // Processing - 取得註冊中使用者的 Model
        var registration = await _registrationRepository.GetAsync(request.Email);
        if (registration == null) throw Errors.Registration.DoFirstStepFirst;
        if (registration.BeenVerified == false) throw Errors.Registration.VerificationUnCorrect;
        
        // Processing - 刪除註冊資料
        await _registrationRepository.DeleteAsync(registration);

        // Processing - 建立新註冊的使用者
        var password = _passwordHashService.Pbkdf2(request.Password);
        var user = _userFactory.Create(name: request.Email, email: request.Email, password: password);

        // Processing - 為使用者建立公司
        var company = _companyFactory.Create(
            name: request.CompanyName,
            location:new Location(request.Latitude,request.Longitude,request.FormattedAddress),
            user: user);
        
        // Processing - 登入
        user.Login(request.DeviceType,request.DeviceToken ?? string.Empty);
        
        // Processing - 保存資料
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.CompanyRepository.AddAsync(company);
        await _unitOfWork.CommitAsync();
        
        // Processing - 計算 Token
        var accessToken = _jwtTokenGenerator.GenerateToken(user.Id,user.SecurityStamp);
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken(user);

        // Return - Token
        return new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}