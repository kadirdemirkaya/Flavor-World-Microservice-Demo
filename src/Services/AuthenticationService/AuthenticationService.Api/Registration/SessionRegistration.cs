namespace AuthenticationService.Api.Registration
{
    public static class Session
    {
        public static IServiceCollection SessionRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(double.Parse(configuration["CookieSettings:ExpiryMinutes"]));
            });

            return services;
        }

        public static WebApplication SessionRegistrationApp(this WebApplication app)
        {
            app.UseSession();

            return app;
        }
    }
}
