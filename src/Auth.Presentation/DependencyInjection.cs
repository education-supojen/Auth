using System.Reflection;
using System.Text;
using Auth.Infrastructure.Authentication;
using Auth.Presentation.Common;
using Auth.Presentation.Middleware;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Auth.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
    {
        #region Configuration 配置
        services.Configure<ClientSettings>(configuration.GetSection(ClientSettings.SectionName));
        #endregion
        
        #region MapSter 配置
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetCallingAssembly());
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        #endregion

        #region 錯誤返回格式(Problem Deatils) 配置
        services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>(); 
        #endregion
        
        #region Authentication 設定
        
        // Processing - 找到 jwt 設定值
        var jwtSettings = new JwtSettings();
        configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);

        // Processing - 建立 JWT 簽名鑰匙
        var tokenSignedKeyBytes = Encoding.UTF8.GetBytes(jwtSettings.Secret);
        var jwtSignedKey = new SymmetricSecurityKey(tokenSignedKeyBytes);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {

                // ================================================================
                // Processing -
                //     如何驗證 JWT
                // ================================================================
                options.TokenValidationParameters = new()
                {
                    // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                    // Processing - 對 Issuer 做認證
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    // Processing - 不對 Audience 做認證
                    ValidateAudience = false,

                    // Processing - 設定 JWT 簽名鑰匙
                    IssuerSigningKey = jwtSignedKey,

                    // Processing - 驗證 Token 的有效期間
                    ValidateLifetime = true ,
                    
                    // Processing - 用 UTC Time 去驗證時間
                    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken _, TokenValidationParameters _) => notBefore <= DateTime.UtcNow && expires >= DateTime.UtcNow,

                    // Processing - 校正時間差
                    ClockSkew = TimeSpan.Zero
                };

                // ================================================================
                // Processing -
                //     驗證 JWT 後, 要做什麼處理
                // ================================================================
                options.Events = new JwtBearerEvents {

                    // ------------------------------------------------------------
                    // Lambda Function -
                    //     當接受到 API 請求時, 還未驗證 JWT Token 前
                    // ------------------------------------------------------------
                    OnMessageReceived = context => {

                        // Processing - 如果是接受匿名的 endpoint, 就不做解讀 JWT 的動作了
                        var endpoint = context.HttpContext.GetEndpoint();
                        var checkAllowAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>();
                        if (checkAllowAnonymous != null)
                        {
                            return Task.CompletedTask;
                        }

                        // Processing - 從 authentication header 取得 Authentication 訊息
                        string authorization = context.Request.Headers["Authorization"];

                        // Processing - 如果沒收到 Authentication 訊息, 發出一個 No Permission 的錯誤訊息
                        if (string.IsNullOrEmpty(authorization)) context.Fail("Invalid");

                            // Processing - 從 authentication header 取出 JWT token
                        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            context.Token = authorization.Substring("Bearer ".Length).Trim();

                        // Processing - 查看是否成功取出 JWT Token
                        if (string.IsNullOrWhiteSpace(context.Token)) context.Fail("Invalid");


                        return Task.CompletedTask;
                    },

                    // ------------------------------------------------------------
                    // Lambda Function -
                    //     Token 驗證失敗時
                    // ------------------------------------------------------------
                    OnChallenge = context =>
                    {

                      // Situation - Token 格式不正確
                      if (context.AuthenticateFailure?.GetType() == typeof(SecurityTokenValidationException))
                        {
                            throw new TokenInvalidException();
                        }
                        // Situation - Token Issuer 不正確
                        if (context.AuthenticateFailure?.GetType() == typeof(SecurityTokenInvalidIssuerException))
                        {
                            throw new TokenInvalidException();
                        }
                        // Situation - Token 期效不正確 (過期或是還不能開始使用)
                        if (context.AuthenticateFailure?.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            throw new TokenExpiredException();
                        }
                        else
                        {
                            throw new TokenInvalidException();
                        }
                    }
                };
            });
        #endregion
        
        #region Swagger 配置
        services.AddSwaggerDocument(settings =>
        {
            //設定文件名稱
            settings.DocumentName = "v1";
            // 設定文件或 API 版本資訊
            settings.Version = $"0.0.0";
            // 設定文件標題 (當顯示 Swagger/ReDoc UI 的時候會顯示在畫面上)
            settings.Title = "Auth 伺服器";
            // 設定文件簡要說明
            settings.Description = "嘗試使用 Clean Architecture 建立 Dotnet REST API";

            var apiScheme = new OpenApiSecurityScheme()
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                //Type = OpenApiSecuritySchemeType.OAuth2,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = @"Copy this into the value field: Bearer {token}
                                    401001: No permission
                                    401002: Invalid Token
                                    401003: Invalid Issuer
                                    401004: Token Expired
                                    401005: Refresh Token Expired"
            };
            settings.AddSecurity("JWT Token", Enumerable.Empty<string>(), apiScheme);
            settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT Token"));
        });
        #endregion
        
        return services;
    }
}