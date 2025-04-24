using MyApp.Application.DTOs;

namespace MyApp.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
    Task<ProductDto> GetProductByIdAsync(string productId);
    Task<ProductListDto> GetProductsAsync(int pageIndex, int pageSize, string? category = null);
    Task<ProductDto> UpdateProductAsync(string productId, UpdateProductDto updateDto);
    Task DeleteProductAsync(string productId);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
    Task<bool> IsProductAvailableAsync(string productId);
    Task<decimal> GetProductPriceAsync(string productId);
}