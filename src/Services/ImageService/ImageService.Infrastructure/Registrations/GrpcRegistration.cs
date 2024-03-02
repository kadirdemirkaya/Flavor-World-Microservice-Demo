using ImageService.Infrastructure.Services.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Infrastructure.Registrations
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
                endpoints.MapGrpcService<UserImageService>();
                
                endpoints.MapGrpcService<ProductImageService>();

                endpoints.MapGet("/Protos/userimage.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/userimage.proto"));
                });

                endpoints.MapGet("/Protos/productimage.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/productimage.proto"));
                });
            });

            return app;
        }
    }
}
