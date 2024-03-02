using BasketService.Api;
using BasketService.Application;
using BasketService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.BasketApiServiceInjection(builder.Configuration);

builder.Services.BasketApplicationServiceInjection(builder.Configuration);

builder.Services.BasketInfrastructureServiceInjection(builder.Configuration);

builder.BasketApiBuilderInjection(builder.Configuration);

var app = builder.Build();

app.BasketApiApplicationInjection(app.Services.GetRequiredService<IHostApplicationLifetime>());

app.BasketInfrastructureApplicationInjection(builder.Services.BuildServiceProvider());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
