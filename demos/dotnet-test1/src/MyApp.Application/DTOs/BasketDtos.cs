namespace MyApp.Application.DTOs;

public record BasketDto(
    string Id,
    string UserId,
    IEnumerable<BasketItemDto> Items,
    decimal Total
);

public record BasketItemDto(
    string Id,
    string ProductId,
    string ProductName,
    decimal ProductPrice,
    int Quantity,
    decimal Subtotal
);

public record AddBasketItemDto(
    string ProductId,
    int Quantity
);

public record UpdateBasketItemDto(
    string ProductId,
    int Quantity
);

public record RemoveBasketItemDto(
    string ProductId
);