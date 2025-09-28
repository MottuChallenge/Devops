using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.Sectors
{
    public class DeleteSectorUseCase
    {
        private readonly ISectorRepository _sectorRepository;
        private readonly IMotorcycleRepository _motorcicleRepository;

        public DeleteSectorUseCase(ISectorRepository sectorRepository, IMotorcycleRepository motorcycleRepository)
        {
            _sectorRepository = sectorRepository;
            _motorcicleRepository = motorcycleRepository;
        }

        public async Task DeleteSectorAsync(Guid sectorId)
        {
            var sector = await _sectorRepository.GetSectorByIdAsync(sectorId);
            if (sector == null)
                throw new KeyNotFoundException($"Sector with ID {sectorId} not found.");

            foreach (var spot in sector.Spots)
            {
                if(spot.MotorcycleId.HasValue && spot.MotorcycleId.Value != null)
                {
                    var motorcycle = await _motorcicleRepository.GetByIdAsync(spot.MotorcycleId.Value);
                    motorcycle.RemoveSpot();
                    spot.RemoveMotorcycle();
                    await _motorcicleRepository.UpdateAsync(motorcycle);
                }
            }
            await _sectorRepository.DeleteAsync(sector);
        }
    }
}