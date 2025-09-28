using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.Sectors
{
    public class GetAllSectorsUseCase
    {
        private readonly ISectorRepository _sectorRepository;

        public GetAllSectorsUseCase(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }

        public async Task<List<SectorResponseDto>> FindAllSectors()
        {
            var sectors = await _sectorRepository.GetAllSectorsAsync();

            return sectors.Select(sector => new SectorResponseDto
            {
                Id = sector.Id,
                YardId = sector.YardId,
                SectorTypeId = sector.SectorTypeId,
                Points = sector.Points.Select(p => new PointResponseDto
                {
                    PointOrder = p.PointOrder,
                    X = p.X,
                    Y = p.Y
                }).ToList(),

                Spots = sector.Spots.Select(s => new SpotResponseDto
                {
                    SpotId = s.SpotId,
                    SectorId = s.SectorId,
                    Status = s.Status,
                    MotorcycleId = s.MotorcycleId,
                    X = s.X,
                    Y = s.Y
                }).ToList()
            }).ToList();
        }

        public async Task<PaginatedResult<SectorResponseDto>> FindAllSectorPageable(
            PageRequest page,
            SectorQuery? filter = null,
            CancellationToken ct = default
        )
        {
            return await _sectorRepository.GetAllSectorPaginated(page, filter, ct);
        }
    }
}
