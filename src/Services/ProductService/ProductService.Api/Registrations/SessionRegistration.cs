﻿namespace ProductService.Api.Registrations
{
    public static class Session
    {
        public static IServiceCollection SessionRegistration(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession();

            return services;
        }

        public static WebApplication SessionRegistrationApp(this WebApplication app)
        {
            app.UseSession();

            return app;
        }
    }
}
