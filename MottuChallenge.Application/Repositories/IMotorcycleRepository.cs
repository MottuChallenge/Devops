using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<Motorcycle> SaveMotorcycleAsync(Motorcycle motorcycle);
        Task<Motorcycle> UpdateAsync(Motorcycle motorcycle);
        Task<PaginatedResult<MotorcycleResponseDto>> GetAllMotorciclePaginated(
            PageRequest page,
            MotorcycleQuery? filter = null,
            CancellationToken ct = default
        );
        Task<Motorcycle> GetByIdAsync(Guid id);
        Task DeleteAsync(Motorcycle motorcycle);

        Task RemoveMotorcyclesByYardId(Guid yardId);
    }
}
