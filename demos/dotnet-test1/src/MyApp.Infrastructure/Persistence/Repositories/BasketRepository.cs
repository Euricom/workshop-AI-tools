namespace MyApp.Infrastructure.Persistence.Repositories;

public class BasketRepository : Repository<Basket>, IBasketRepository
{
    public BasketRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Basket?> GetByUserIdAsync(string userId)
    {
        return await Query()
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task<bool> HasItemAsync(string basketId, string productId)
    {
        return await _context.BasketItems
            .AnyAsync(i => i.Id == basketId && i.ProductId == productId);
    }

    public override async Task<Basket?> GetByIdAsync(string id)
    {
        return await Query()
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public override async Task<IEnumerable<Basket>> GetAllAsync()
    {
        return await Query()
            .Include(b => b.Items)
            .ToListAsync();
    }

    public override async Task<Basket> AddAsync(Basket basket)
    {
        // Check if user already has a basket
        var existingBasket = await GetByUserIdAsync(basket.UserId);
        if (existingBasket != null)
            throw new Application.Exceptions.DuplicateEntityException(
                nameof(Basket), 
                $"User {basket.UserId} already has a basket");

        return await base.AddAsync(basket);
    }

    public override async Task UpdateAsync(Basket basket)
    {
        var existing = await GetByIdAsync(basket.Id);
        if (existing == null)
            throw new Application.Exceptions.NotFoundException(nameof(Basket), basket.Id);

        // Ensure we're not changing the user ID
        if (existing.UserId != basket.UserId)
            throw new Application.Exceptions.ValidationException(
                new Dictionary<string, string[]>
                {
                    { "UserId", new[] { "Cannot change basket owner" } }
                });

        await base.UpdateAsync(basket);
    }

    protected override IQueryable<Basket> Query()
    {
        return base.Query()
            .AsSplitQuery() // Split the query to avoid cartesian explosion
            .Include(b => b.Items);
    }
}