namespace MyApp.Core.Interfaces;

public interface IUnitOfWork
{
    IRepository<Entities.Product> Products { get; }
    IRepository<Entities.Customer> Customers { get; }
    IRepository<Entities.Order> Orders { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}