namespace MyApp.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}

public interface IUserRepository : IRepository<Entities.User>
{
    Task<Entities.User?> GetByUsernameAsync(string username);
    Task<bool> IsUsernameUniqueAsync(string username);
    Task<bool> IsEmailUniqueAsync(string email);
}

public interface IProductRepository : IRepository<Entities.Product>
{
    Task<IEnumerable<Entities.Product>> GetByCategoryAsync(string category);
    Task<IEnumerable<Entities.Product>> GetAvailableAsync();
    Task<(IEnumerable<Entities.Product> Items, int TotalCount)> GetPagedAsync(int pageIndex, int pageSize, string? category = null);
}

public interface IBasketRepository : IRepository<Entities.Basket>
{
    Task<Entities.Basket?> GetByUserIdAsync(string userId);
    Task<bool> HasItemAsync(string basketId, string productId);
}