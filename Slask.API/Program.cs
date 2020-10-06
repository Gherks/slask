using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Slask.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(options =>
                {
                    options.ValidateScopes = true; // Don't do this when deploying to production
                })
                .UseStartup<Startup>();
    }
}
