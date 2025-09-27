using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.Helpers;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Application.UseCases.Motorcycles
{
    public class UpdateMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ISectorRepository _sectorRepository;

        public UpdateMotorcycleUseCase(IMotorcycleRepository motorcycleRepository, ISectorRepository sectorRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _sectorRepository = sectorRepository;
        }

        public async Task<Motorcycle> UpdateMotorcycleAsync(Guid motorcycleId, MotorcycleDto dto)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null)
                throw new KeyNotFoundException($"Motorcycle with ID {motorcycleId} not found.");

            motorcycle.UpdateModel(dto.Model);
            motorcycle.UpdateEngineType(dto.EngineType);
            motorcycle.UpdatePlate(dto.Plate);
            motorcycle.UpdateLastRevisionDate(dto.LastRevisionDate);

            if (motorcycle.SpotId.HasValue)
            {
                var oldSector = await _sectorRepository.GetSectorBySpotId(motorcycle.SpotId.Value);
                var oldSpot = oldSector.Spots.FirstOrDefault(s => s.SpotId == motorcycle.SpotId.Value);
                oldSpot?.RemoveMotorcycle();
                motorcycle.RemoveSpot();
                await _sectorRepository.UpdateAsync(oldSector);
            }

           
            if (dto.SpotId.HasValue && dto.SpotId.Value != Guid.Empty)
            {
                var sector = await _sectorRepository.GetSectorBySpotId(dto.SpotId.Value);
                ValidateIfExistMotorcycleInsideSpot.ValidateMotorcycleInsideSpot(sector, dto.SpotId.Value);
                var spot = sector.Spots.FirstOrDefault(s => s.SpotId == dto.SpotId.Value);
                if (spot == null)
                    throw new KeyNotFoundException("Spot not found in the specified sector.");

                spot.AssignMotorcycle(motorcycle);
                await _sectorRepository.UpdateAsync(sector);
            }

            await _motorcycleRepository.UpdateAsync(motorcycle);

            return motorcycle;
        }

    }
}
