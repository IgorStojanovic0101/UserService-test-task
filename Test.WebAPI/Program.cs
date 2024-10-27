using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Compact;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.Features;

using Microsoft.AspNetCore.SignalR;
using Test.Application;
using Test.Infrastructure;
using Test.WebAPI.Hubs;
using Test.WebAPI.Utilities.middlewares;
using Test.WebAPI.Utilities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Solution", Version = "v1" });
});




builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddSignalR();


builder.Services.AddMvc(options => options.Conventions.Add(new RouteConvention()));

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();




builder.Services.AddLogging(loggingBuilder =>
{

    var logsPath = builder.Environment.IsDevelopment() ? Path.Combine("D:\\Projects\\.NET Core\\Room\\UserService-test-task\\Test.WebAPI\\Logs", "logs.txt")
                                                        :Path.Combine(AppContext.BaseDirectory, "Logs", "logs.txt");
    var logger = new LoggerConfiguration()
             .ReadFrom.Configuration(builder.Configuration)
             .Enrich.WithThreadId()
             .WriteTo.File(logsPath,
                           rollingInterval: RollingInterval.Infinite,
                           outputTemplate: "{Timestamp:MM/dd/yyyy H:mm:ss zzzz} {ThreadId} {Level} {SourceContext} {Message:lj}{NewLine}{Exception}")
             .CreateLogger();

    loggingBuilder.AddSerilog(logger);
});



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Solution v1");
    });
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();


app.UseHttpsRedirection();


app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();
app.MapHub<SocketHub>("/SocketHub");
app.Run();
