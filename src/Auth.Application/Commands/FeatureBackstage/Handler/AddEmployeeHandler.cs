using Auth.Application.Interfaces.Persistence;
using Auth.Domain.Aggregates;
using Auth.Domain.Enums;
using Auth.Domain.Factories.Interface;
using MediatR;

namespace Auth.Application.Commands.FeatureBackstage.Handler;

public class AddEmployeeHandler : IRequestHandler<AddEmployeeCommand, User>
{
    private readonly IUserFactory _userFactory;
    private readonly IUnitOfWork _unitOfWork;

    public AddEmployeeHandler(IUserFactory userFactory, IUnitOfWork unitOfWork) => (_userFactory,_unitOfWork) = (userFactory,unitOfWork);
    
    
    public async Task<User> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Processing - 搜尋公司
        var company = await _unitOfWork.CompanyRepository.GetAsync(request.CompanyId);
        
        // Processing - 建立使用者
        var user = await _userFactory.CreateAsync(request.Name, request.Email, request.Title, request.BoardingTime);
        
        // Processing - 把使用者加入公司, 並且指定權限
        switch (request.Permission)
        {
            case Permission.admin:
                company.AddUserAsAdmin(user);
                break;
            case Permission.backstage:
                company.AddUserAsBackstageManager(user);
                break;
            case Permission.manager:
                company.AddUserAsManager(user);
                break;
            case Permission.employee:
                company.AddUserAsEmployee(user);
                break;
        }

        // Processing - 處存變更
        await _unitOfWork.BeginTransactionAsync();
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        // Return - 使用者
        return user;
    }
}