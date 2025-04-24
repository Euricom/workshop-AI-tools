namespace MyApp.Application.Common.Configuration;

public class ApplicationSettings
{
    public JwtSettings Jwt { get; set; } = new();
    public ValidationSettings Validation { get; set; } = new();
    public PaginationSettings Pagination { get; set; } = new();
}

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 60;
}

public class ValidationSettings
{
    public PasswordValidationSettings Password { get; set; } = new();
    public int MaxBasketItems { get; set; } = 20;
    public int MaxItemQuantity { get; set; } = 10;
}

public class PasswordValidationSettings
{
    public int MinimumLength { get; set; } = 8;
    public bool RequireDigit { get; set; } = true;
    public bool RequireLowercase { get; set; } = true;
    public bool RequireUppercase { get; set; } = true;
    public bool RequireNonAlphanumeric { get; set; } = true;
}

public class PaginationSettings
{
    public int DefaultPageSize { get; set; } = 10;
    public int MaxPageSize { get; set; } = 100;
}

public static class ApplicationConstants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class ErrorMessages
    {
        public const string InvalidCredentials = "Invalid username or password";
        public const string UserNotFound = "User not found";
        public const string ProductNotFound = "Product not found";
        public const string BasketNotFound = "Shopping basket not found";
        public const string ProductNotAvailable = "Product is not available";
        public const string MaxBasketItemsReached = "Maximum number of different items in basket reached";
        public const string MaxItemQuantityReached = "Maximum quantity for this item reached";
    }

    public static class CacheKeys
    {
        public const string ProductList = "products";
        public const string ProductDetails = "product-{0}"; // {0} = productId
        public const string UserBasket = "basket-{0}"; // {0} = userId
    }
}