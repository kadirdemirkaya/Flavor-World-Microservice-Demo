using OrderService.Api;
using OrderService.Application;
using OrderService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.OrderApplicationBuilderInjection(builder.Configuration);

builder.Services.OrderApplicationServiceInjection(builder.Configuration);

builder.Services.OrderApiServiceInjection(builder.Configuration);

builder.Services.OrderInfrastructureServiceInjection(builder.Configuration);

var app = builder.Build();

app.UseRouting();

app.OrderApiApplicationInjection(builder.Configuration, app.Services.GetRequiredService<IHostApplicationLifetime>());

app.OrderInfrastructureApplicationInjection(builder.Services.BuildServiceProvider());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
