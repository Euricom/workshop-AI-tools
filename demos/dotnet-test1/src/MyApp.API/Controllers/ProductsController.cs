using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;
using MyApp.Application.Common.Configuration;
using MyApp.Application.Common.Models;

namespace MyApp.API.Controllers;

public class ProductsController : ApiControllerBase
{
    private readonly IProductService _productService;
    private readonly ApplicationSettings _settings;

    public ProductsController(
        IProductService productService,
        ApplicationSettings settings)
    {
        _productService = productService;
        _settings = settings;
    }

    /// <summary>
    /// Get paginated list of products
    /// </summary>
    /// <param name="pageIndex">Page number (1-based)</param>
    /// <param name="pageSize">Items per page</param>
    /// <param name="category">Optional category filter</param>
    /// <returns>List of products with pagination metadata</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? category = null)
    {
        pageSize = Math.Min(pageSize, _settings.Pagination.MaxPageSize);
        var result = await _productService.GetProductsAsync(pageIndex, pageSize, category);
        
        var paginatedResult = new PaginatedResult<ProductDto>(
            result.Items.ToList(),
            result.TotalCount,
            pageIndex,
            pageSize);

        return FromPaginatedResult(paginatedResult);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <returns>Product details</returns>
    [HttpGet("{productId}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        return product != null ? Ok(product) : NotFound();
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="createDto">Product creation details</param>
    /// <returns>Created product details</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto createDto)
    {
        var product = await _productService.CreateProductAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { productId = product.Id }, product);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <param name="updateDto">Product update details</param>
    /// <returns>Updated product details</returns>
    [HttpPut("{productId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(string productId, [FromBody] UpdateProductDto updateDto)
    {
        var product = await _productService.UpdateProductAsync(productId, updateDto);
        return Ok(product);
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{productId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string productId)
    {
        await _productService.DeleteProductAsync(productId);
        return NoContent();
    }

    /// <summary>
    /// Get products by category
    /// </summary>
    /// <param name="category">Category name</param>
    /// <returns>List of products in the category</returns>
    [HttpGet("category/{category}")]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var products = await _productService.GetProductsByCategoryAsync(category);
        return Ok(products);
    }
}