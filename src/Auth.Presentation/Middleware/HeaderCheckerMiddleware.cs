using Microsoft.Extensions.Options;

namespace Auth.Presentation.Middleware;

public class HeaderCheckerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ClientSettings settings;

    public HeaderCheckerMiddleware(RequestDelegate next,IOptions<ClientSettings> options)
    {
        this.next = next;
        settings = options.Value;
    }
    
    
    public async Task Invoke(HttpContext httpContext)
    {
        // Variables - 
        var clientId = httpContext.Request.Headers["ClientId"];
        var clientSecret = httpContext.Request.Headers["ClientSecret"];
        
        // Processing - 
        if (string.IsNullOrEmpty(clientId))
        {
            httpContext.Response.Redirect("/error/id");
        }
        else
        {
            // Processing - 
            var client = settings.Clients.FirstOrDefault(x => x.Id == clientId);
        
            // Processing - 
            if (client is null)
            {
                httpContext.Response.Redirect("/error/id");
            }
        
            // Processing - 
            if (client!.Secret != clientSecret)
            {
                httpContext.Response.Redirect("/error/secret");
            }
        }
        
        
        // Processing - 
        await next(httpContext);
    }
}

public static class HeadersCheckersExtensions
{
    public static IApplicationBuilder CheckHeader(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HeaderCheckerMiddleware>();
    }
}