using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.Repositories
{
    public interface IYardRepository
    {
        Task<Yard> SaveYardAsync(Yard yard);

        Task<Yard?> GetYardByIdAsync(Guid id);

        Task<List<Yard>> GetAllYardsAsync();

        Task UpdateAsync(Yard yard);

        Task DeleteAsync(Yard yard);

        Task<PaginatedResult<YardResponseDto>> GetAllYardPaginated(
            PageRequest page,
            YardQuery? filter = null,
            CancellationToken ct = default);


    }
}
