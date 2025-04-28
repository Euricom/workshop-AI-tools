using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;

namespace MyApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Gets all customers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    /// <summary>
    /// Gets a customer by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        return Ok(customer); // NotFoundException will be handled by middleware if not found
    }

    /// <summary>
    /// Creates a new customer
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto createCustomerDto)
    {
        var customer = await _customerService.CreateCustomerAsync(createCustomerDto);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Updates an existing customer
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerDto>> Update(Guid id, [FromBody] UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
        return Ok(customer);
    }

    /// <summary>
    /// Deletes a customer
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}