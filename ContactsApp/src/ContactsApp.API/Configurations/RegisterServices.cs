using ContactsApp.Core.Interfaces;
using ContactsApp.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace ContactsApp.API.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class RegisterServices
    {
        public static IServiceCollection ServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if(configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Register Services Dependency Injection
            services.AddScoped<IContactService, ContactService>();

            return services;
        }
    }
}
