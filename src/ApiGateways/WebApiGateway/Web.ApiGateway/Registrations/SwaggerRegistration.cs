﻿namespace Web.ApiGateway.Registrations
{
    public static class Swagger
    {
        public static IServiceCollection SwaggerRegistration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = Constants.ApiGateway.Application.ApplicationName,
                    Version = Constants.ApiGateway.Application.Version,
                    Description = Constants.ApiGateway.Application.Description,
                });
            });
            return services;
        }

        public static WebApplication SwaggerRegistrationApp(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlavorWorld API v1");
                });
            }

            return app;
        }
    }
}
