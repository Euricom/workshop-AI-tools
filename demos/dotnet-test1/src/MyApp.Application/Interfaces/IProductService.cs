using MyApp.Application.DTOs;

namespace MyApp.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto);
    Task DeleteProductAsync(Guid id);
    Task<bool> SetProductAvailabilityAsync(Guid id, bool isAvailable);
    Task<ProductDto> AddStockAsync(Guid id, int quantity);
    Task<ProductDto> RemoveStockAsync(Guid id, int quantity);
    Task<ProductDto> UpdateReorderPointAsync(Guid id, int reorderPoint);
}