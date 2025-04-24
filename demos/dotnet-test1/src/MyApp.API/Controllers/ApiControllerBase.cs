using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Common.Models;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess && result.Data != null)
            return Ok(result.Data);

        if (result.IsSuccess && result.Data == null)
            return NotFound();

        return BadRequest(result.Error);
    }

    protected ActionResult<TResponse> FromResult<TResponse>(Result<TResponse> result)
    {
        if (result.IsSuccess && result.Data != null)
            return Ok(result.Data);

        if (result.IsSuccess && result.Data == null)
            return NotFound();

        return BadRequest(result.Error);
    }

    protected IActionResult FromPaginatedResult<T>(PaginatedResult<T> result)
    {
        var metadata = new
        {
            result.TotalCount,
            result.PageSize,
            result.TotalPages,
            result.PageIndex,
            result.HasNextPage,
            result.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

        return Ok(result.Items);
    }

    protected string? GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    protected string? GetUsername()
    {
        return User.FindFirst(ClaimTypes.Name)?.Value;
    }

    protected bool IsInRole(string role)
    {
        return User.IsInRole(role);
    }
}