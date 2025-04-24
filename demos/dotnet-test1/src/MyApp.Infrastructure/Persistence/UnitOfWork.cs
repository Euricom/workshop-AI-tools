namespace MyApp.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    IBasketRepository Baskets { get; }
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _disposed;

    private IUserRepository? _users;
    private IProductRepository? _products;
    private IBasketRepository? _baskets;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => 
        _users ??= new Repositories.UserRepository(_context);

    public IProductRepository Products => 
        _products ??= new Repositories.ProductRepository(_context);

    public IBasketRepository Baskets => 
        _baskets ??= new Repositories.BasketRepository(_context);

    public async Task BeginTransactionAsync()
    {
        await _context.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.RollbackTransactionAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
            _disposed = true;
        }
    }
}