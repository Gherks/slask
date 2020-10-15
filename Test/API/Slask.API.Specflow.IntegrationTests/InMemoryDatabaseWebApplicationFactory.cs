using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Slask.Persistence;
using System;

namespace Slask.API.Specflow.IntegrationTests
{
    public class InMemoryDatabaseWebApplicationFactory<StartupType> : WebApplicationFactory<StartupType> where StartupType : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                ServiceProvider serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<SlaskContext>(options =>
                    {
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                        options.UseInternalServiceProvider(serviceProvider);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    },
                    ServiceLifetime.Transient,
                    ServiceLifetime.Transient);

                ServiceProvider builtServiceProvider = services.BuildServiceProvider();

                using (IServiceScope scope = builtServiceProvider.CreateScope())
                {
                    IServiceProvider scopedServiceProvider = scope.ServiceProvider;
                    SlaskContext slaskContext = scopedServiceProvider.GetRequiredService<SlaskContext>();

                    slaskContext.Database.EnsureCreated();
                }
            });
        }
    }
}
