using Api.Context;
using Api.DataInitializers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests;

internal class FirstApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            DataInitializerControl.SkipInitData = true;
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddNpgsql<ApplicationDbContext>("Host=localhost;Port=5432;Database=tests_tasks_database;Username=postgres;Password=admin;Include Error Detail=true;");
            var context = CreateDbContext(services);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });
        base.ConfigureWebHost(builder);
    }

    private static ApplicationDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
    
}