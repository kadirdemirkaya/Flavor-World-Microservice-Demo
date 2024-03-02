using Web.ApiGateway.Registrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.CorsRegistration();

builder.Services.OcelotRegistration();

builder.Services.SwaggerRegistration();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Configurations/ocelot.json")
    .AddEnvironmentVariables();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.SwaggerRegistrationApp();
}

app.UseHttpsRedirection();

app.CorsRegistrationApp();

app.OcelotRegistrationApp();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
