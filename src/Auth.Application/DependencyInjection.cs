using Auth.Application.Factories.Aggregates;
using Auth.Domain.Factories.Aggregates;
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
        services.AddSingleton<IPasswordUpdateFactory, PasswordUpdateFactory>();
        services.AddSingleton<IUserFactory, UserFactory>();
        services.AddSingleton<IStaffFactory, StaffFactory>();
        #endregion
        
        return services;
    }
}