using Microsoft.AspNetCore.Builder;
using ProductService.Infrastructure.Middlewares;

namespace ProductService.Infrastructure.Registrations
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
