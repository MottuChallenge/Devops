using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Application.UseCases.Motorcycles
{
    public class DeleteMotorcycleUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ISectorRepository _sectorRepository;

        public DeleteMotorcycleUseCase(IMotorcycleRepository motorcycleRepository, ISectorRepository sectorRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _sectorRepository = sectorRepository;
        }

        public async Task DeleteMotorcycleAsync(Guid motorcycleId)
        {
            var motorcycle = await _motorcycleRepository.GetByIdAsync(motorcycleId);
            if (motorcycle == null)
                throw new KeyNotFoundException($"Motorcycle with ID {motorcycleId} not found.");

            if (motorcycle.SpotId.HasValue && motorcycle.SpotId.Value != Guid.Empty)
            {
                var sector = await _sectorRepository.GetSectorBySpotId(motorcycle.SpotId.Value);

                motorcycle.RemoveSpot();
                await _sectorRepository.UpdateAsync(sector);
            }
            
            await _motorcycleRepository.DeleteAsync(motorcycle);
        }
    }
}
