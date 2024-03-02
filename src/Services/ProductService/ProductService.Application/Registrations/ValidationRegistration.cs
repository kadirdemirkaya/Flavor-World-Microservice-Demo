using BuildingBlock.Validator;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Common.Behaviors;

namespace ProductService.Application.Registrations
{
    public static class Validation
    {
        public static IServiceCollection ValidationRegistrationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.ValidatorExtension(AssemblyReference.Assembly);

            services.AddFluentValidationAutoValidation(opt =>
            {
                opt.DisableDataAnnotationsValidation = true;
            }).AddValidatorsFromAssembly(AssemblyReference.Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
