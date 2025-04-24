using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;

namespace MyApp.API.Controllers;

[Authorize]
public class BasketsController : ApiControllerBase
{
    private readonly IBasketService _basketService;

    public BasketsController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    /// <summary>
    /// Get current user's basket
    /// </summary>
    /// <returns>Basket details with items</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBasket()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var basket = await _basketService.GetBasketAsync(userId);
        return basket != null ? Ok(basket) : NotFound();
    }

    /// <summary>
    /// Add item to basket
    /// </summary>
    /// <param name="itemDto">Item details to add</param>
    /// <returns>Updated basket details</returns>
    [HttpPost("items")]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem([FromBody] AddBasketItemDto itemDto)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var basket = await _basketService.AddItemToBasketAsync(userId, itemDto);
        return Ok(basket);
    }

    /// <summary>
    /// Update basket item quantity
    /// </summary>
    /// <param name="itemDto">Item update details</param>
    /// <returns>Updated basket details</returns>
    [HttpPut("items")]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromBody] UpdateBasketItemDto itemDto)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var basket = await _basketService.UpdateBasketItemAsync(userId, itemDto);
        return Ok(basket);
    }

    /// <summary>
    /// Remove item from basket
    /// </summary>
    /// <param name="itemDto">Item to remove</param>
    /// <returns>Updated basket details</returns>
    [HttpDelete("items")]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem([FromBody] RemoveBasketItemDto itemDto)
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var basket = await _basketService.RemoveItemFromBasketAsync(userId, itemDto);
        return Ok(basket);
    }

    /// <summary>
    /// Clear all items from basket
    /// </summary>
    /// <returns>No content</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearBasket()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        await _basketService.ClearBasketAsync(userId);
        return NoContent();
    }

    /// <summary>
    /// Validate basket items availability
    /// </summary>
    /// <returns>Validation result</returns>
    [HttpGet("validate")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateBasket()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var isValid = await _basketService.ValidateBasketItemsAvailabilityAsync(userId);
        return Ok(isValid);
    }
}