using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Core.Entities;
using MyApp.Core.ValueObjects;

namespace MyApp.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

            logger.LogInformation("Ensuring database is created...");
            
            if (context.Database.IsSqlite())
            {
                await context.Database.MigrateAsync();
            }
            else
            {
                await context.Database.MigrateAsync();
            }

            await SeedDataAsync(context, logger);

            logger.LogInformation("Database initialization completed.");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    private static async Task SeedDataAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Products.AnyAsync())
        {
            logger.LogInformation("Seeding products...");
            var products = new List<Product>
            {
                new Product(
                    name: "Laptop",
                    description: "High-performance laptop",
                    price: Money.Create(999.99m),
                    stockQuantity: 10
                ),
                new Product(
                    name: "Smartphone",
                    description: "Latest smartphone model",
                    price: Money.Create(699.99m),
                    stockQuantity: 15
                )
            };
            await context.Products.AddRangeAsync(products);
        }

        if (!await context.Customers.AnyAsync())
        {
            logger.LogInformation("Seeding customers...");
            var customers = new List<Customer>
            {
                new Customer(
                    name: "John Doe",
                    email: Email.Create("john.doe@example.com"),
                    phoneNumber: PhoneNumber.Create("+1234567890")
                ),
                new Customer(
                    name: "Jane Smith",
                    email: Email.Create("jane.smith@example.com"),
                    phoneNumber: PhoneNumber.Create("+1987654321")
                )
            };
            await context.Customers.AddRangeAsync(customers);
        }

        await context.SaveChangesAsync();
    }
}