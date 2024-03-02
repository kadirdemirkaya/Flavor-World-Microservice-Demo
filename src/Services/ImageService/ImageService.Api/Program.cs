using ImageService.Api;
using ImageService.Application;
using ImageService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ImageApiServiceInjection(builder.Configuration);

builder.ImageApiBuilderInjection(builder.Configuration);

builder.Services.ImageApplicationServiceInjection(builder.Configuration);

builder.ImageApplicationBuilderInjection(builder.Configuration);

builder.Services.ImageInfrastructureServiceInjection(builder.Configuration);

builder.ImageInfrastructureBuilderInjection(builder.Configuration);

var app = builder.Build();

app.ImageApiApplicationInjection(app.Services.GetRequiredService<IHostApplicationLifetime>());

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.ImageInfrastructureApplicationInjection();

app.MapControllers();

app.Run();
