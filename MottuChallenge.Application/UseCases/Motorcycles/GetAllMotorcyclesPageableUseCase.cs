using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.Motorcycles
{
    public class GetAllMotorcyclesPageableUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        public GetAllMotorcyclesPageableUseCase(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }
        public async Task<PaginatedResult<MotorcycleResponseDto>> FindAllMotorcyclePageable(
            PageRequest page,
            MotorcycleQuery? filter = null,
            CancellationToken ct = default
        )
        {
            return await _motorcycleRepository.GetAllMotorciclePaginated(page, filter, ct);
        }
    }
}
