using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;
using MyApp.Core.Entities;
using MyApp.Core.Interfaces;
using MyApp.Application.Common.Exceptions;

namespace MyApp.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        return order == null ? null : MapToDto(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(Guid customerId)
    {
        var orders = await _unitOfWork.Orders.FindAsync(o => o.CustomerId == customerId);
        return orders.Select(MapToDto);
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(createOrderDto.CustomerId);
        if (customer == null)
            throw new NotFoundException(nameof(Customer), createOrderDto.CustomerId);

        var order = customer.CreateOrder();

        foreach (var itemDto in createOrderDto.Items)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
            if (product == null)
                throw new NotFoundException(nameof(Product), itemDto.ProductId);

            order.AddItem(product, itemDto.Quantity);
        }

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(order);
    }

    public async Task<OrderDto> AddItemToOrderAsync(Guid orderId, CreateOrderItemDto itemDto)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);

        var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
        if (product == null)
            throw new NotFoundException(nameof(Product), itemDto.ProductId);

        order.AddItem(product, itemDto.Quantity);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(order);
    }

    public async Task<OrderDto> UpdateOrderItemAsync(Guid orderId, UpdateOrderItemDto itemDto)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);

        order.UpdateItemQuantity(itemDto.ProductId, itemDto.Quantity);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(order);
    }

    public async Task<OrderDto> RemoveItemFromOrderAsync(Guid orderId, Guid productId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);

        order.RemoveItem(productId);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(order);
    }

    public async Task<OrderDto> ConfirmOrderAsync(Guid orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);

        order.Confirm();
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(order);
    }

    public async Task<OrderDto> CancelOrderAsync(Guid orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);

        order.Cancel();
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(order);
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer.Name,
            OrderStatus = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            ConfirmedAt = order.ConfirmedAt,
            CancelledAt = order.CancelledAt,
            TotalAmount = order.TotalAmount.Amount,
            Items = order.Items.Select(item => new OrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice.Amount,
                SubTotal = item.SubTotal.Amount
            }).ToList()
        };
    }
}