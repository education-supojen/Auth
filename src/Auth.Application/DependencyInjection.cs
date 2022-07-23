using Auth.Application.Factories;
using Auth.Domain.Factories;
using Auth.Domain.Factories.Interface;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        #region MediatR 配置
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        #endregion
        
        #region Domain Factory 配置
        services.AddSingleton<IRegistrationFactory, RegistrationFactory>();
        services.AddSingleton<IUserFactory, UserFactory>();
        services.AddSingleton<ICompanyFactory, CompanyFactory>();
        services.AddSingleton<IDepartmentFactory, DepartmentFactory>();
        services.AddSingleton<IScheduleFactory, ScheduleFactory>();
        services.AddSingleton<IShiftFactory, ShiftFactory>();
        services.AddSingleton<IPunchGroupFactory, PunchGroupFactory>();
        #endregion
        
        return services;
    }
}