using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Helpers;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
namespace MottuChallenge.Application.UseCases.Motorcycles
{
    public class CreateMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ISectorRepository _sectorRepository;

        public CreateMotorcycleUseCase(IMotorcycleRepository motorcycleRepository, ISectorRepository sectorRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _sectorRepository = sectorRepository;
        }

        public async Task<MotorcycleResponseDto> SaveMotorcycleAsync(MotorcycleDto motorcycleDto)
        {
            var motorcycle = new Motorcycle(motorcycleDto.Model, motorcycleDto.EngineType, motorcycleDto.Plate, motorcycleDto.LastRevisionDate);
            await _motorcycleRepository.SaveMotorcycleAsync(motorcycle);

            if (motorcycleDto.SpotId.HasValue && motorcycleDto.SpotId.Value != Guid.Empty)
            {
                var sector = await _sectorRepository.GetSectorBySpotId(motorcycleDto.SpotId.Value);

                ValidateIfExistMotorcycleInsideSpot.ValidateMotorcycleInsideSpot(sector, motorcycleDto.SpotId.Value);
            
                sector.AssignMotorcycleToSpot(motorcycleDto.SpotId.Value, motorcycle);
                await _sectorRepository.UpdateAsync(sector);
                await _motorcycleRepository.UpdateAsync(motorcycle);
            }

            return new MotorcycleResponseDto
            {
                Id = motorcycle.Id,
                Model = motorcycle.Model,
                EngineType = motorcycle.EngineType,
                Plate = motorcycle.Plate,
                LastRevisionDate = motorcycle.LastRevisionDate,
                SpotId = motorcycle.SpotId
            }
            ;
        }

       

    }
}
