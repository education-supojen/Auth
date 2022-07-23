using Auth.Application.Interfaces.Services;
using Auth.Domain.Aggregates;
using Auth.Domain.Factories.Interface;
using Auth.Domain.ValueObjects;

namespace Auth.Application.Factories;

public class CompanyFactory : ICompanyFactory
{
    private readonly IDepartmentFactory _departmentFactory;
    private readonly IScheduleFactory _scheduleFactory;
    private readonly IShiftFactory _shiftFactory;
    private readonly IPunchGroupFactory _punchGroupFactory;
    private readonly IIdentityProducer _identityProducer;

    public CompanyFactory(
        IDepartmentFactory departmentFactory,
        IScheduleFactory scheduleFactory,
        IShiftFactory shiftFactory,
        IPunchGroupFactory punchGroupFactory,
        IIdentityProducer identityProducer)
    {
        _departmentFactory = departmentFactory;
        _scheduleFactory = scheduleFactory;
        _shiftFactory = shiftFactory;
        _punchGroupFactory = punchGroupFactory;
        _identityProducer = identityProducer;
    }
    
    /// <summary>
    /// 建立公司
    /// </summary>
    /// <param name="name">公司的名稱</param>
    /// <param name="location">公司的位置</param>
    /// <param name="user">建立公司的使用者</param>
    /// <returns></returns>
    public Company Create(string name, Location location, User user)
    {
        var id = _identityProducer.GetId();
        var department = _departmentFactory.Create();
        var schedule = _scheduleFactory.Create();
        var shift = _shiftFactory.Create();
        var punchGroup = _punchGroupFactory.Create(location);
        
        var company = new Company(id, name, location);
        company.AddDepartment(department);
        company.AddSchedule(schedule);
        company.AddShift(shift);
        company.AddPunchGroup(punchGroup);
        
        company.AddUserAsAdmin(user);
        user.AssignUserToDepartment(department);
        user.AssignUserToSchedule(schedule);
        user.AssignUserToShift(shift);
        user.AssignUserToPunchGroup(punchGroup);

        return company;
    }
}