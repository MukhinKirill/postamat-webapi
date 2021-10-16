using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PostamatService.Data.Repositories;
using PostamatService.Web;

namespace PostamatService.Tests
{
    public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RepositoryContext>));

                if (dbContext != null)
                    services.Remove(dbContext);

                var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<RepositoryContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryOrderTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                using var appContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
                {
                    appContext.Database.EnsureCreated();
                }
            });
        }
    }
}
