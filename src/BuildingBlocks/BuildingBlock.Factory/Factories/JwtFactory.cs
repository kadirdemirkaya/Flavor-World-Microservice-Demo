using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BuildingBlock.Factory.Factories
{
    public static class JwtFactory
    {
        public static IJwtTokenGenerator CreateJwtTokenGenerator(IConfiguration configuration)
        {
            return new JwtTokenGenerator(configuration);
        }

        public static ITokenService CreateTokenService(IServiceProvider serviceProvider, IHttpContextAccessor httpContext)
        {
            return new TokenService(serviceProvider, httpContext);
        }

        public static ITokenService CreateTokenService(IServiceProvider serviceProvider, IHttpContextAccessor httpContext, DatabaseConfig config, DbContext context)
        {
            return new TokenService(serviceProvider, httpContext, config, context);
        }
    }
}
