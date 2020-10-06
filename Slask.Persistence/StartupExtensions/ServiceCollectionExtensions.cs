using Microsoft.Extensions.DependencyInjection;
using Slask.Application.Interfaces.Persistence;
using Slask.Persistence.Repositories;

namespace Slask.Persistence.StartupExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddTransient<UserRepositoryInterface, UserRepository>();
            services.AddTransient<TournamentRepositoryInterface, TournamentRepository>();
            return services;
        }
    }
}
