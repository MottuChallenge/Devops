using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Infrastructure.Persistence;

namespace MottuChallenge.Infrastructure.Repositories;

internal class EmployeeRepository(MottuChallengeContext context) : IEmployeeRepository
{
    private readonly MottuChallengeContext _context = context;

    public async Task<Employee?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == email, ct);
    }

    public Task AddAsync(Employee employee, CancellationToken ct = default)
    {
        _context.Employees.Add(employee);
        return _context.SaveChangesAsync(ct);
    }
}