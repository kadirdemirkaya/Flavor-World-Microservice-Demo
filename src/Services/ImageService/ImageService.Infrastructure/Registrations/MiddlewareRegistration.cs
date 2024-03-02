using ImageService.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ImageService.Infrastructure.Registrations
{
    public static class Middleware
    {
        public static WebApplication MiddlewareRegistrationApp(this WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            return app;
        }
    }
}
