using MyApp.Core.Entities;
using MyApp.Core.Interfaces;
using MyApp.Infrastructure.Persistence.Repositories;

namespace MyApp.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IRepository<Product>? _products;
    private IRepository<Customer>? _customers;
    private IRepository<Order>? _orders;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<Product> Products => 
        _products ??= new BaseRepository<Product>(_context);

    public IRepository<Customer> Customers =>
        _customers ??= new BaseRepository<Customer>(_context);

    public IRepository<Order> Orders =>
        _orders ??= new BaseRepository<Order>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
}