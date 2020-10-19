using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slask.Persistence;
using System;
using System.Linq;

namespace Slask.API.Specflow.IntegrationTests.Utilities
{
    public class InMemoryDatabaseWebApplicationFactory<StartupType> : WebApplicationFactory<StartupType> where StartupType : class
    {
        private readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot = new InMemoryDatabaseRoot();
        private readonly string _testDatabaseName = "InMemoryTestDatabase_" + Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                ServiceDescriptor slaskContextDescriptor = services.SingleOrDefault(descriptor =>
                    descriptor.ServiceType == typeof(DbContextOptions<SlaskContext>));

                if (slaskContextDescriptor != null)
                {
                    services.Remove(slaskContextDescriptor);
                }

                services.AddDbContext<SlaskContext>(options =>
                    {
                        options.UseInMemoryDatabase(_testDatabaseName, _inMemoryDatabaseRoot);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    },
                    ServiceLifetime.Transient,
                    ServiceLifetime.Transient);

                ServiceProvider builtServiceProvider = services.BuildServiceProvider();

                using (IServiceScope scope = builtServiceProvider.CreateScope())
                {
                    IServiceProvider scopedServiceProvider = scope.ServiceProvider;
                    SlaskContext slaskContext = scopedServiceProvider.GetRequiredService<SlaskContext>();
                    var logger = scopedServiceProvider.GetRequiredService<ILogger<InMemoryDatabaseWebApplicationFactory<StartupType>>>();

                    slaskContext.Database.EnsureCreated();

                    try
                    {
                        // Seed database here if ever needed.
                    }
                    catch (Exception exception)
                    {
                        const string errorMessage = "An error ocurred when seeding the database with test data. Error: {Message}";
                        logger.LogError(exception, errorMessage, exception.Message);
                    }
                }
            });
        }
    }
}
