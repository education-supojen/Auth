using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Errors;
using Auth.Domain.Factories.Aggregates;
using MediatR;
using OneOf;

namespace Auth.Application.Commands.Feature.Updating.Handler;

public class UpdatePasswordStep1Handler : IRequestHandler<UpdatePasswordStep1Command,OneOf<bool,Failure>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordUpdateFactory _factory;

    public UpdatePasswordStep1Handler(IUnitOfWork unitOfWork,IPasswordUpdateFactory factory)
    {
        _unitOfWork = unitOfWork;
        _factory = factory;
    }
    
    
    public async Task<OneOf<bool,Failure>> Handle(UpdatePasswordStep1Command request, CancellationToken cancellationToken)
    {
        // ----------------------------------------------------------------------------------------------------
        // Begin Transaction 
        // ----------------------------------------------------------------------------------------------------
        await _unitOfWork.BeginTransactionAsync();
        
        // Processing - 建立更新密碼資料
        var passwordUpdate = _factory.Create(request.Email);

        // Processing - 處存更新密碼資料
        await _unitOfWork.PasswordUpdateRepository.AddAsync(passwordUpdate);
        
        // ----------------------------------------------------------------------------------------------------
        // End Transaction 
        // ----------------------------------------------------------------------------------------------------
        var commit = await _unitOfWork.CommitAsync();

        // Processing - commit failure
        if (commit.IsT1) return commit.AsT1;
        
        // Mission Complete
        return true;
    }
}