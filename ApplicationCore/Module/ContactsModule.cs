using ApplicationCore.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Module;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CreatePersonDtoValidator>();
        services.AddFluentValidationAutoValidation();
        return services;
    }
}
