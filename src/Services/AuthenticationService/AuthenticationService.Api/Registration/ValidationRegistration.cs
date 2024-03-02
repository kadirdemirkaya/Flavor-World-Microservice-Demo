using BuildingBlock.Validator;

namespace AuthenticationService.Api.Registration
{
    public static class Validation
    {
        public static IServiceCollection ValidationRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.ValidatorExtension(AssemblyReference.Assembly);

            return services;
        }
    }
}
