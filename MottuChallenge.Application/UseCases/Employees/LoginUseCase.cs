using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.UseCases.Employees;

public class LoginUseCase(IEmployeeRepository _employeeRepository)
{
    public async Task<Employee?> LoginEmployee(string email, string password, CancellationToken ct = default)
    {
        var employee = await _employeeRepository.GetByEmailAsync(email, ct);
        return !employee.VerifyPassword(password) ? null : employee;
    }
}