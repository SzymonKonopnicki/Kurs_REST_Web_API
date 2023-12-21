using RestaurantAPI.Entities;
using RestaurantAPI.Interfaces;
using RestaurantAPI.Services;
using NLog;
using NLog.Web;
using RestaurantAPI.Middleware;
using System.Diagnostics;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddDbContext<RestaurantDbContext>();
    builder.Services.AddScoped<RestaurantSeeds>();
    builder.Services.AddAutoMapper(typeof(Program).Assembly);
    builder.Services.AddScoped<IRestaurantServices, RestaurantServices>();
    builder.Services.AddScoped<ErrorMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();
    builder.Services.AddScoped<Stopwatch>();
    builder.Services.AddScoped<IDishServices, DishServices>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

    builder.Host.UseNLog();

    var app = builder.Build();

    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeds>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseMiddleware<ErrorMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();

}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
