using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Slask.Application.Utilities;
using Slask.Persistence;
using Slask.Persistence.StartupExtensions;
using System;

namespace Slask.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration.GetConnectionString("SqlServer");

            services.AddDbContext<SlaskContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                },
                ServiceLifetime.Transient,
                ServiceLifetime.Transient)
            .AddPersistenceServices()
            .AddHandlers()
            .AddSingleton<CommandQueryDispatcher>()
            .AddControllers(configure =>
            {
                configure.ReturnHttpNotAcceptable = true;
            }).Services
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            UpdateDatabaseAsync(app);

            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabaseAsync(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<SlaskContext>())
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
