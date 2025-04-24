using MyApp.Application.DTOs;

namespace MyApp.Application.Interfaces;

public interface IBasketService
{
    Task<BasketDto> GetBasketAsync(string userId);
    Task<BasketDto> AddItemToBasketAsync(string userId, AddBasketItemDto itemDto);
    Task<BasketDto> UpdateBasketItemAsync(string userId, UpdateBasketItemDto itemDto);
    Task<BasketDto> RemoveItemFromBasketAsync(string userId, RemoveBasketItemDto itemDto);
    Task<bool> ValidateBasketItemsAvailabilityAsync(string userId);
    Task ClearBasketAsync(string userId);
}