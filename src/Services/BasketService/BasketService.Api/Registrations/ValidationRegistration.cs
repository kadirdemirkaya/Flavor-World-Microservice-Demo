using BuildingBlock.Validator;

namespace BasketService.Api.Registrations
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
