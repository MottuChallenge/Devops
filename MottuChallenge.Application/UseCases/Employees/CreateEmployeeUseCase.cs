using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.UseCases.Employees;

public class CreateEmployeeUseCase(IEmployeeRepository employeeRepository, IYardRepository yardRepository)
{
    public async Task<Employee> createEmployee(string name, string email, Guid yardId, string password)
    {
        var yard = await yardRepository.GetYardByIdAsync(yardId);
        if (yard == null)
        {
            throw new Exception("Yard not found");
        }
        
        var employee = new Employee(name, email, yard, password);
        await employeeRepository.AddAsync(employee);
        return employee;
    }
}