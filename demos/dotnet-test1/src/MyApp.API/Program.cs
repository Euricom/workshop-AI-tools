using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyApp.API.Middleware;
using MyApp.Application.Interfaces;
using MyApp.Application.Services;
using MyApp.Core.Interfaces;
using MyApp.Infrastructure.Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OnionShop API",
        Version = "v1",
        Description = "API for the OnionShop e-commerce application",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@onionshop.com"
        }
    });

    // Include XML comments in Swagger documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Infrastructure services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Application services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnionShop API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at application root
    });
}

// Global exception handling
app.UseExceptionHandling();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    try
    {
        await DatabaseInitializer.InitializeDatabaseAsync(scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
        throw;
    }
}

app.Run();