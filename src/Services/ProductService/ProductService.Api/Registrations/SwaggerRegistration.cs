using Microsoft.OpenApi.Models;

namespace ProductService.Api.Registrations
{
    public static class Swagger
    {
        public static IServiceCollection SwaggerRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = ProductService.Domain.Constants.Constant.App.ApplicationName,
                    Version = ProductService.Domain.Constants.Constant.App.Version,
                    Description = ProductService.Domain.Constants.Constant.App.Description,
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
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
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{ProductService.Domain.Constants.Constant.App.ApplicationName} API {ProductService.Domain.Constants.Constant.App.Version}");
                });
            }

            return app;
        }
    }
}
