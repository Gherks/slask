using Microsoft.Extensions.DependencyInjection;
using Slask.Domain.Services;
using System.Reflection;

namespace Slask.Domain.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            //services.AddTransient(typeof(PlayersService));
            services.AddTransient(typeof(PlayersService).Assembly);
            //services.AddTransient<PlayersService, PlayersService>();
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
