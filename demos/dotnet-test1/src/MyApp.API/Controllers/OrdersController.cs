using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gets all orders
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Gets an order by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetById(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }

    /// <summary>
    /// Gets orders for a specific customer
    /// </summary>
    [HttpGet("customer/{customerId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetCustomerOrders(Guid customerId)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId);
        return Ok(orders);
    }

    /// <summary>
    /// Creates a new order
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto createOrderDto)
    {
        var order = await _orderService.CreateOrderAsync(createOrderDto);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Adds an item to an order
    /// </summary>
    [HttpPost("{orderId:guid}/items")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> AddItem(Guid orderId, [FromBody] CreateOrderItemDto itemDto)
    {
        var updatedOrder = await _orderService.AddItemToOrderAsync(orderId, itemDto);
        return Ok(updatedOrder);
    }

    /// <summary>
    /// Updates an item in an order
    /// </summary>
    [HttpPut("{orderId:guid}/items")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> UpdateItem(Guid orderId, [FromBody] UpdateOrderItemDto itemDto)
    {
        var updatedOrder = await _orderService.UpdateOrderItemAsync(orderId, itemDto);
        return Ok(updatedOrder);
    }

    /// <summary>
    /// Removes an item from an order
    /// </summary>
    [HttpDelete("{orderId:guid}/items/{productId:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> RemoveItem(Guid orderId, Guid productId)
    {
        var updatedOrder = await _orderService.RemoveItemFromOrderAsync(orderId, productId);
        return Ok(updatedOrder);
    }

    /// <summary>
    /// Confirms an order
    /// </summary>
    [HttpPost("{orderId:guid}/confirm")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> ConfirmOrder(Guid orderId)
    {
        var confirmedOrder = await _orderService.ConfirmOrderAsync(orderId);
        return Ok(confirmedOrder);
    }

    /// <summary>
    /// Cancels an order
    /// </summary>
    [HttpPost("{orderId:guid}/cancel")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> CancelOrder(Guid orderId)
    {
        var cancelledOrder = await _orderService.CancelOrderAsync(orderId);
        return Ok(cancelledOrder);
    }
}