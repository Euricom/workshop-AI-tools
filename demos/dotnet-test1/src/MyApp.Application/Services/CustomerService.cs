using MyApp.Application.DTOs;
using MyApp.Application.Interfaces;
using MyApp.Core.Entities;
using MyApp.Core.Interfaces;
using MyApp.Application.Common.Exceptions;
using MyApp.Core.ValueObjects;

namespace MyApp.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        // Check if email is already in use
        var existingCustomer = await _unitOfWork.Customers
            .FindAsync(c => c.Email.Value == createCustomerDto.Email);
        
        if (existingCustomer.Any())
            throw new InvalidOperationException("Email is already in use");

        var customer = new Customer(
            createCustomerDto.Name,
            Email.Create(createCustomerDto.Email),
            PhoneNumber.Create(createCustomerDto.PhoneNumber)
        );

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(Guid id, UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
            throw new NotFoundException(nameof(Customer), id);

        // Check if email is already in use by another customer
        if (updateCustomerDto.Email != customer.Email.Value)
        {
            var existingCustomer = await _unitOfWork.Customers
                .FindAsync(c => c.Email.Value == updateCustomerDto.Email && c.Id != id);
            
            if (existingCustomer.Any())
                throw new InvalidOperationException("Email is already in use by another customer");
        }

        customer.UpdateName(updateCustomerDto.Name);
        customer.UpdateContactInfo(
            Email.Create(updateCustomerDto.Email),
            PhoneNumber.Create(updateCustomerDto.PhoneNumber)
        );

        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
            throw new NotFoundException(nameof(Customer), id);

        // Check if customer has any orders
        var hasOrders = customer.Orders.Any();
        if (hasOrders)
            throw new InvalidOperationException("Cannot delete customer with existing orders");

        await _unitOfWork.Customers.DeleteAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CustomerDto> GetCustomerByEmailAsync(string email)
    {
        var customers = await _unitOfWork.Customers.FindAsync(c => c.Email.Value == email);
        var customer = customers.FirstOrDefault();
        
        if (customer == null)
            throw new NotFoundException("Customer", $"email: {email}");

        return MapToDto(customer);
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email.Value,
            PhoneNumber = customer.PhoneNumber.Value
        };
    }
}