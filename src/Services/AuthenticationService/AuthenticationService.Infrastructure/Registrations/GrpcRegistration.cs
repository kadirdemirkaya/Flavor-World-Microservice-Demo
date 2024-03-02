using AuthenticationService.Infrastructure.Services.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Infrastructure.Registrations
{
    public static class Grpc
    {
        public static IServiceCollection GrpcRegistration(this IServiceCollection services)
        {
            services.AddGrpc();

            return services;
        }

        public static WebApplication GrpcRegistrationApp(this WebApplication app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserInfoService>();

                endpoints.MapGet("/Protos/userinfo.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/userinfo.proto"));
                });
            });

            return app;
        }
    }
}
