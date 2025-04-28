using MyApp.Application.DTOs;

namespace MyApp.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<CustomerDto> UpdateCustomerAsync(Guid id, UpdateCustomerDto updateCustomerDto);
    Task DeleteCustomerAsync(Guid id);
    Task<CustomerDto> GetCustomerByEmailAsync(string email);
}