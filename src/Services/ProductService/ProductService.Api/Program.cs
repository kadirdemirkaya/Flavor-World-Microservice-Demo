using ProductService.Api;
using ProductService.Application;
using ProductService.Domain.Constants;
using ProductService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.ProductApiServiceInjection(builder.Configuration)
                .ProductApplicationServiceInjection(builder.Configuration)
                .ProductInfrastructureServiceInjection(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constant.Roles.User, policy => policy.RequireRole(Constant.Roles.User));
    options.AddPolicy(Constant.Roles.Moderator, policy => policy.RequireRole(Constant.Roles.Moderator));
    options.AddPolicy(Constant.Roles.Guest, policy => policy.RequireRole(Constant.Roles.Guest));
    options.AddPolicy(Constant.Roles.Boss, policy => policy.RequireRole(Constant.Roles.Boss));
    options.AddPolicy(Constant.Roles.Admin, policy => policy.RequireRole(Constant.Roles.Admin));
});

builder.ProductInfrastructureBuilderInjection(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.ProductApiApplicationInjection(app.Services.GetRequiredService<IHostApplicationLifetime>());

app.ProductInfrastructureApplicationInjection(builder.Services.BuildServiceProvider(), builder.Configuration);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
