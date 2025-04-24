using MyApp.Application.DTOs;

namespace MyApp.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> RegisterUserAsync(UserRegistrationDto registration);
    Task<UserDto> GetUserByIdAsync(string userId);
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
}

public interface IAuthenticationService
{
    Task<AuthenticationResultDto> AuthenticateAsync(UserLoginDto loginDto);
    Task<string> GenerateJwtTokenAsync(string userId, string username);
    bool ValidateToken(string token);
}

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Username { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}