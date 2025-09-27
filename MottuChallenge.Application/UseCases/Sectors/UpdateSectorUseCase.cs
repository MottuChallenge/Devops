using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Helpers;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Application.UseCases.Sectors
{
    public class UpdateSectorUseCase
    {
        private readonly ISectorRepository _sectorRepository;
        private readonly ISectorTypeRepository _sectorTypeRepository;

        public UpdateSectorUseCase(ISectorRepository sectorRepository, ISectorTypeRepository sectorTypeRepository)
        {
            _sectorRepository = sectorRepository;
            _sectorTypeRepository = sectorTypeRepository;
        }

        public async Task<SectorResponseDto> UpdateSectorTypeAsync(Guid sectorId, UpdateSectorDto dto)
        {
            var sector = await _sectorRepository.GetSectorByIdAsync(sectorId);
            if (sector == null)
                throw new KeyNotFoundException($"Sector with ID {sectorId} not found.");

            var sectorType = await _sectorTypeRepository.FindByIdAsync(dto.SectorTypeId);
            if (sectorType == null)
                throw new KeyNotFoundException($"SectorType with ID {dto.SectorTypeId} not found.");

            sector.AddSectorType(sectorType);
            await _sectorRepository.UpdateAsync(sector);

            return new SectorResponseDto
            {
                Id = sector.Id,
                YardId = sector.YardId,
                SectorTypeId = sector.SectorTypeId,
                Points = PolygonPointsMapping.CreateListOfPointResponseDto(sector.Points),
                Spots = sector.Spots.Select(s => new SpotResponseDto { SpotId = s.SpotId, SectorId = s.SectorId, Status = s.Status, MotorcycleId = s.MotorcycleId, X = s.X, Y = s.Y }).ToList()
            };
        }
    }
}
