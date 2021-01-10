using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Services;


namespace ServicesLib
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddScoped<IPeopleService, PeopleService>();
            

            return services;
        }
    }
}
