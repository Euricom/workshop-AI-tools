using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;
using MyApp.Core.Entities;
using MyApp.Core.Interfaces;
using MyApp.Application.Common.Exceptions;
using MyApp.Core.ValueObjects;

namespace MyApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = new Product(
            createProductDto.Name,
            createProductDto.Description,
            Money.Create(createProductDto.Price),
            createProductDto.StockQuantity,
            createProductDto.ReorderPoint
        );

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(product);
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        product.UpdateDetails(
            updateProductDto.Name,
            updateProductDto.Description,
            Money.Create(updateProductDto.Price)
        );

        if (updateProductDto.StockQuantity.HasValue)
        {
            var currentStock = product.StockQuantity;
            var difference = updateProductDto.StockQuantity.Value - currentStock;
            
            if (difference > 0)
                product.AddStock(difference);
            else if (difference < 0)
                product.RemoveStock(Math.Abs(difference));
        }

        if (updateProductDto.ReorderPoint.HasValue)
        {
            product.UpdateReorderPoint(updateProductDto.ReorderPoint.Value);
        }

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(product);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        await _unitOfWork.Products.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> SetProductAvailabilityAsync(Guid id, bool isAvailable)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return false;

        try
        {
            product.SetAvailability(isAvailable);
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public async Task<ProductDto> AddStockAsync(Guid id, int quantity)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        product.AddStock(quantity);
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(product);
    }

    public async Task<ProductDto> RemoveStockAsync(Guid id, int quantity)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        product.RemoveStock(quantity);
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(product);
    }

    public async Task<ProductDto> UpdateReorderPointAsync(Guid id, int reorderPoint)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            throw new NotFoundException(nameof(Product), id);

        product.UpdateReorderPoint(reorderPoint);
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(product);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Amount,
            IsAvailable = product.IsAvailable,
            StockQuantity = product.StockQuantity,
            ReorderPoint = product.ReorderPoint,
            NeedsReorder = product.NeedsReorder()
        };
    }
}