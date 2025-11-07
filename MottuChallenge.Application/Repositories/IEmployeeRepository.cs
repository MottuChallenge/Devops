using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(Employee employee, CancellationToken ct = default);
}