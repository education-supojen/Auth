using System.Text.Json;
using System.Text.Json.Serialization;
using Auth.Application;
using Auth.Infrastructure;
using Auth.Presentation;
using Auth.Presentation.Middleware;
using Serilog;


var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Application Starting Up");

    var builder = WebApplication.CreateBuilder(args);
    {
        builder.Host.UseSerilog();

        builder.Services
            .AddControllers()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());  // Enum 轉成文字, 先試試看好不好用
            });
        
        
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration)
            .AddPresentation(builder.Configuration);
        
    }

    var app = builder.Build();
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }

        app.CheckHeader();
        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly.");
}
finally
{
    Log.CloseAndFlush();
}





