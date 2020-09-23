using Microsoft.Extensions.DependencyInjection;
using Slask.Persistence.Repositories;
using System.Reflection;

namespace Slask.Persistence.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(UserRepository).Assembly);
            services.AddTransient(typeof(TournamentRepository).Assembly);
            return services;
        }

        public static IServiceCollection AddTransient(this IServiceCollection services, Assembly assembly)
        {
            foreach (var serviceType in assembly.ExportedTypes)
            {
                var typeInfo = serviceType.GetTypeInfo();
                if (typeInfo.IsClass && typeInfo.IsAbstract == false)
                {
                    services.AddTransient(serviceType, serviceType);
                }
            }

            return services;
        }
    }
}
