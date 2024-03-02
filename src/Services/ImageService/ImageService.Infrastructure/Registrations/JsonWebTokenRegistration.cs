﻿using BuildingBlock.Base.Abstractions;
using BuildingBlock.Factory.Factories;
using BuildingBlock.Jwt;
using ImageService.Application.Configurations;
using ImageService.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Infrastructure.Registrations
{
    public static class JsonWebToken
    {
        public static IServiceCollection JsonWebTokenServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.JwtExtension(configuration);

            services.AddScoped<IJwtTokenGenerator>(sp =>
            {
                return JwtFactory.CreateJwtTokenGenerator(configuration);
            });

            var sp = GetServiceProvider(services);

            var httpContext = sp.GetRequiredService<IHttpContextAccessor>();
            var context = sp.GetRequiredService<ImageDbContext>();

            services.AddScoped<ITokenService>(sp =>
            {
                return JwtFactory.CreateTokenService(sp, httpContext, GetConfigs.GetDatabaseConfig, context);
            });

            return services;
        }

        private static ServiceProvider GetServiceProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
