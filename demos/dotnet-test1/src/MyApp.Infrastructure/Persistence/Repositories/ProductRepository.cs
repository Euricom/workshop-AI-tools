namespace MyApp.Infrastructure.Persistence.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await WhereAsync(p => p.Category == category);
    }

    public async Task<IEnumerable<Product>> GetAvailableAsync()
    {
        return await WhereAsync(p => p.Available);
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int pageIndex, 
        int pageSize, 
        string? category = null)
    {
        var query = Query();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public override async Task<Product> AddAsync(Product product)
    {
        // Additional validation could be added here
        if (product.Price < 0)
            throw new Application.Exceptions.ValidationException(
                new Dictionary<string, string[]>
                {
                    { "Price", new[] { "Price cannot be negative" } }
                });

        return await base.AddAsync(product);
    }

    public override async Task UpdateAsync(Product product)
    {
        var existing = await GetByIdAsync(product.Id);
        if (existing == null)
            throw new Application.Exceptions.NotFoundException(nameof(Product), product.Id);

        // Additional validation could be added here
        if (product.Price < 0)
            throw new Application.Exceptions.ValidationException(
                new Dictionary<string, string[]>
                {
                    { "Price", new[] { "Price cannot be negative" } }
                });

        await base.UpdateAsync(product);
    }
}