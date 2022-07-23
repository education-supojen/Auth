using Auth.Application.Interfaces.Persistence;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Errors;
using Auth.Domain.Factories.Aggregates;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Register.Handler;

public class StaffRegistrationStep1Handler : IRequestHandler<StaffRegistrationStep1Command,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailMediator _emailMediator;
    private readonly IRegistrationFactory _registrationFactory;


    public StaffRegistrationStep1Handler(
        IUnitOfWork unitOfWork,
        IEmailMediator emailMediator,
        IRegistrationFactory registrationFactory)
    {
        _unitOfWork = unitOfWork;
        _emailMediator = emailMediator;
        _registrationFactory = registrationFactory;
    }
    
    /// <summary>
    /// 註冊 - 第一步
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OneOf<bool,Failure>> Handle(StaffRegistrationStep1Command request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Begin Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 建立註冊資訊
        var registration = _registrationFactory.Create(request.Email);
        
        // Processing - 處存註冊資料
        await _unitOfWork.RegistrationRepository.AddAsync(registration);
        
        // Processing - 發送驗證碼
        await _emailMediator.StaffRegistrationAsync(request.Email, registration.Code);

        // ----------------------------------------------------------------------------------------------------
        // End Transaction 
        // ----------------------------------------------------------------------------------------------------
        var commit = await _unitOfWork.CommitAsync();

        // Processing - Commit Failure
        if (commit.IsT1) return commit.AsT1;
        
        // Mission Complete
        return true;
    }
}