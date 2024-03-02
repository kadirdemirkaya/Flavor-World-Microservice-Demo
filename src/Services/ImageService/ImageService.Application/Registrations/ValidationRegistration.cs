using BuildingBlock.Validator;
using FluentValidation;
using FluentValidation.AspNetCore;
using ImageService.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.Application.Registrations
{
    public static class Validation
    {
        public static IServiceCollection ValidationRegistration(this IServiceCollection services, IConfiguration configuration)
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
