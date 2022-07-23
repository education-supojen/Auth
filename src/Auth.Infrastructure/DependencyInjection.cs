using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Enums;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Authentication;
using Auth.Infrastructure.Persistence.Dapper;
using Auth.Infrastructure.Persistence.EF;
using Auth.Infrastructure.Persistence.Redis;
using Auth.Infrastructure.Persistence.Repositories;
using Auth.Infrastructure.Redis;
using Auth.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using StackExchange.Redis;

namespace Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region Configuration 配置
        services.Configure<SendGridSettings>(configuration.GetSection(SendGridSettings.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<AesSettings>(configuration.GetSection(AesSettings.SectionName));
        #endregion
        
        #region Service 配置
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddSingleton<IEmailMediator, EmailMediator>();
        services.AddSingleton<IAesService, AesService>();
        services.AddSingleton<IPasswordHashService, PasswordHashService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IIdentityProducer>(_ => new IdentityProducer(new Random().Next(1,31),new Random().Next(1,31)));
        services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
        #endregion

        #region Repository 配置
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<IPasswordUpdateRepository, PasswordUpdateRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        #endregion

        #region Unit Of Work 配置
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region Dapper 配置
        services.AddScoped(_ => new NpgsqlConnection(configuration.GetConnectionString("Postgres")));
        InjectMiniORMMapping.Inject();
        services.AddScoped<IMiniORMDatabase, MiniORMDatabase>();
        #endregion

        #region EF Core 配置
        services.AddDbContext<AppDbContext>(ctx => 
            ctx.UseLazyLoadingProxies().UseNpgsql(configuration.GetConnectionString("Postgres")));
        #endregion
        
        #region Postgres 配置
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<DayOfWeek>();
        #endregion
        
        #region MediatR 配置
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        #endregion

        #region Redis 配置
        services.AddSingleton((_) => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
        services.AddScoped<IAggregateCache, AggregateCache>((context) =>
        {
            var connect = context.GetService<ConnectionMultiplexer>();
            return new AggregateCache(connect);
        });
        services.AddScoped<ICache>((context) =>
        {
            var connect = context.GetService<ConnectionMultiplexer>();
            return new RedisCache(connect);
        });
        services.AddScoped<IRedisCache>((context) =>
        {
            var connect = context.GetService<ConnectionMultiplexer>();
            return new RedisCache(connect);
        });
        services.AddScoped<ICacheLinkList>((context) =>
        {
            var connect = context.GetService<ConnectionMultiplexer>();
            return new RedisCache(connect);
        });
        services.AddScoped<ICacheLocker>((context) =>
        {
            var connect = context.GetService<ConnectionMultiplexer>();
            return new RedisCache(connect);
        });
        #endregion

        return services;
    }
}