using BSYS.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BSYS.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection collection)
    {
        collection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
        collection.AddHttpClient();
        collection.AddFluentValidationAutoValidation();
        collection.AddValidatorsFromAssemblyContaining<BaseValidator>();
    }
}