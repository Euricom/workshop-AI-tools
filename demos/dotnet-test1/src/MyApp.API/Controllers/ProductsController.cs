using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Gets all products
    /// </summary>
    /// <returns>A list of all products</returns>
    /// <response code="200">Returns the list of products</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Gets a product by ID
    /// </summary>
    /// <param name="id">The ID of the product to retrieve</param>
    /// <returns>The requested product</returns>
    /// <response code="200">Returns the requested product</response>
    /// <response code="404">If the product is not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return Ok(product); // NotFoundException will be handled by middleware if not found
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="createProductDto">The product information</param>
    /// <returns>The newly created product</returns>
    /// <response code="201">Returns the newly created product</response>
    /// <response code="400">If the product information is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto createProductDto)
    {
        var product = await _productService.CreateProductAsync(createProductDto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">The ID of the product to update</param>
    /// <param name="updateProductDto">The updated product information</param>
    /// <returns>The updated product</returns>
    /// <response code="200">Returns the updated product</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="400">If the product information is invalid</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        var product = await _productService.UpdateProductAsync(id, updateProductDto);
        return Ok(product);
    }

    /// <summary>
    /// Deletes a product
    /// </summary>
    /// <param name="id">The ID of the product to delete</param>
    /// <returns>No content</returns>
    /// <response code="204">If the product was successfully deleted</response>
    /// <response code="404">If the product is not found</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Sets the availability of a product
    /// </summary>
    /// <param name="id">The ID of the product</param>
    /// <param name="isAvailable">The availability status to set</param>
    /// <returns>No content</returns>
    /// <response code="204">If the availability was successfully updated</response>
    /// <response code="404">If the product is not found</response>
    [HttpPatch("{id:guid}/availability")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> SetAvailability(Guid id, [FromBody] bool isAvailable)
    {
        await _productService.SetProductAvailabilityAsync(id, isAvailable);
        return NoContent();
    }
}