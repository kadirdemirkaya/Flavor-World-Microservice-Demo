namespace Web.ApiGateway.Registrations
{
    public static class Cors
    {
        public static IServiceCollection CorsRegistration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });


            return services;
        }

        public static WebApplication CorsRegistrationApp(this WebApplication app)
        {
            app.UseCors("AllowAllOrigins");

            return app;
        }
    }
}
