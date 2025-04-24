using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;

namespace MyApp.API.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authService;

    public UsersController(
        IUserService userService,
        IAuthenticationService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="registration">User registration details</param>
    /// <returns>Created user details</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto registration)
    {
        var result = await _userService.RegisterUserAsync(registration);
        return CreatedAtAction(nameof(GetById), new { userId = result.Id }, result);
    }

    /// <summary>
    /// Authenticate user and get JWT token
    /// </summary>
    /// <param name="login">Login credentials</param>
    /// <returns>Authentication result with JWT token</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto login)
    {
        var result = await _authService.AuthenticateAsync(login);
        return Ok(result);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{userId}")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return user != null ? Ok(user) : NotFound();
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    /// <returns>Current user details</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetUserId();
        if (userId == null)
            return Unauthorized();

        var user = await _userService.GetUserByIdAsync(userId);
        return user != null ? Ok(user) : NotFound();
    }
}