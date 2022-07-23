using Auth.Application.Interfaces.Authentication;
using Auth.Application.Interfaces.Persistence;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Enums;
using Auth.Domain.Repositories;
using Auth.Infrastructure.Authentication;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.Dapper;
using Auth.Infrastructure.Persistence.EF;
using Auth.Infrastructure.Persistence.Redis;
using Auth.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SendGrid;
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
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        #endregion

        #region Unit Of Work 配置
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region MediatR 配置
        services.AddMediatR(typeof(DependencyInjection).Assembly);
        #endregion

        #region Dapper 配置
        services.AddScoped(_ => new NpgsqlConnection(configuration.GetConnectionString("Postgres")));
        MiniORMMapping.CreateMapping();
        services.AddScoped<IMiniORMDatabase, MiniOrmDatabase>();
        #endregion

        #region EF Core 配置
        services.AddDbContext<AppDbContext>(ctx => 
            ctx.UseNpgsql(configuration.GetConnectionString("Postgres")));
        #endregion
        
        #region Postgres 配置
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        NpgsqlConnection.GlobalTypeMapper.MapEnum<DayOfWeek>();
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Permission>();
        #endregion

        #region Redis 配置
        services.AddSingleton((_) => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
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