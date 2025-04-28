using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Core.Interfaces;
using MyApp.Infrastructure.Persistence;

namespace MyApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                // Use in-memory SQLite for development/testing
                options.UseSqlite("DataSource=:memory:");
            }
            else
            {
                // Use provided connection string
                options.UseSqlite(connectionString);
            }
        });

        // Register repositories and unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        await DatabaseInitializer.InitializeDatabaseAsync(serviceProvider);
    }
}