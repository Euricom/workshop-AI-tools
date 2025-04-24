namespace MyApp.Application.DTOs;

public record UserDto(
    string Id,
    string Username,
    string Email,
    DateTime CreatedAt
);

public record UserRegistrationDto(
    string Username,
    string Email,
    string Password
);

public record UserLoginDto(
    string Username,
    string Password
);

public record AuthenticationResultDto(
    string Token,
    string Username,
    DateTime ExpiresAt
);