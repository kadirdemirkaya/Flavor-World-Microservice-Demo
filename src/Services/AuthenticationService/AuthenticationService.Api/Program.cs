using AuthenticationService.Api;
using AuthenticationService.Application;
using AuthenticationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AuthenticationApiServiceInjection(builder.Configuration);

builder.Services.AuthenticationInfrastructureServiceInjection(builder.Configuration);

builder.Services.AuthenticationApplicationServiceInjection(builder.Configuration);

builder.AuthenticationApiBuilderInjection(builder.Configuration);

builder.AuthenticationApplicationBuilderInjection(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.AuthenticationApiApplicationInjection(app.Services.GetRequiredService<IHostApplicationLifetime>());

app.AuthenticationInfrastructureApplicationInjection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
