using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BuildingBlock.Swagger
{
    public static class Extension
    {
        public static IServiceCollection SwaggerServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = configuration["SwaggerSettings:Version"],
                    Title = configuration["SwaggerSettings:Title"],
                    Description = configuration["SwaggerSettings:Description"]
                });

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter ‘Bearer’ [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        public static WebApplication SwaggerApplicationRegistration(this WebApplication app, IConfiguration configuration)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{configuration["SwaggerSettings:Swagger"]}/{configuration["SwaggerSettings:Version"]}/{configuration["SwaggerSettings:Swagger"]}.json", $"{configuration["SwaggerSettings:Title"]}{configuration["SwaggerSettings:Version"]}FlavorWorld API v1");
            });
            return app;
        }
    }
}
