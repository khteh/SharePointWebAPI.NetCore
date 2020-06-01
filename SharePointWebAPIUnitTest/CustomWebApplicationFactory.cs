using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using SharePointWebAPI.NetCore;
using SharePointWebAPI.NetCore.Models;

namespace SharePointWebAPIUnitTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        // use whatever config you want here
                        webBuilder.UseStartup<Startup>()
                            .UseTestServer()
                            .ConfigureServices(services =>
                            {
                                // Create a new service provider.
                                var serviceProvider = new ServiceCollection()
                                    .AddEntityFrameworkInMemoryDatabase().AddLogging()
                                    .BuildServiceProvider();
                                // Add a database context (AppDbContext) using an in-memory database for testing.
                                services.AddDbContextPool<SharePointContext>(options =>
                                {
                                    options.UseInMemoryDatabase("InMemoryAppDb");
                                    options.UseInternalServiceProvider(serviceProvider);
                                });
                                // Build the service provider.
                                var sp = services.BuildServiceProvider();
                                // Create a scope to obtain a reference to the database contexts
                                using (var scope = sp.CreateScope())
                                {
                                    var scopedServices = scope.ServiceProvider;
                                    var appDb = scopedServices.GetRequiredService<SharePointContext>();
                                    // Ensure the database is created.
                                    appDb.Database.EnsureCreated();
                                }
                            });
                    });
        }
    }
}