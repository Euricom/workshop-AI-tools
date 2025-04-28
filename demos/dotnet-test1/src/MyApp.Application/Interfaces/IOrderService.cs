using MyApp.Application.DTOs;

namespace MyApp.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto?> GetOrderByIdAsync(Guid id);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(Guid customerId);
    Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<OrderDto> AddItemToOrderAsync(Guid orderId, CreateOrderItemDto itemDto);
    Task<OrderDto> UpdateOrderItemAsync(Guid orderId, UpdateOrderItemDto itemDto);
    Task<OrderDto> RemoveItemFromOrderAsync(Guid orderId, Guid productId);
    Task<OrderDto> ConfirmOrderAsync(Guid orderId);
    Task<OrderDto> CancelOrderAsync(Guid orderId);
}