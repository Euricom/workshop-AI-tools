namespace MyApp.Application.DTOs;

public record ProductDto(
    string Id,
    string Name,
    string Description,
    decimal Price,
    bool Available,
    string Category
);

public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    bool Available,
    string Category
);

public record UpdateProductDto(
    string? Name = null,
    string? Description = null,
    decimal? Price = null,
    bool? Available = null,
    string? Category = null
);

public record ProductListDto(
    IEnumerable<ProductDto> Items,
    int TotalCount,
    int PageIndex,
    int PageSize,
    string? Category
);