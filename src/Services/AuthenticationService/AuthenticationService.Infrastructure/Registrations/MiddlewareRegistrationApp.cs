using AuthenticationService.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AuthenticationService.Infrastructure.Registrations
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
