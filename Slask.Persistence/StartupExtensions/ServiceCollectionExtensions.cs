using Microsoft.Extensions.DependencyInjection;
using Slask.Persistence.Services;
using System.Reflection;

namespace Slask.Persistence.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(UserService).Assembly);
            services.AddTransient(typeof(TournamentService).Assembly);
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
