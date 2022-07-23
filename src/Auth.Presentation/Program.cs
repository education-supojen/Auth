using System.Text.Json;
using Auth.Application;
using Auth.Infrastructure;
using Auth.Presentation;
using Auth.WebApi;
using Auth.WebApi.Filters;
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
            .AddControllers(opt => opt.Filters.Add<ExceptionFilter>())
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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





