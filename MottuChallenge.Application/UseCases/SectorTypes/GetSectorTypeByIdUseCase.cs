using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;


namespace MottuChallenge.Application.UseCases.SectorTypes
{
    public class GetSectorTypeByIdUseCase
    {
        private readonly ISectorTypeRepository _sectorTypeRepository;
        public GetSectorTypeByIdUseCase(ISectorTypeRepository sectorTypeRepository)
        {
            _sectorTypeRepository = sectorTypeRepository;
        }
        public async Task<SectorType> FindSectorTypeById(Guid id)
        {
            var sectorType = await _sectorTypeRepository.FindByIdAsync(id);
            if (sectorType == null)
                throw new KeyNotFoundException("SectorType not found.");
            return sectorType;
        }
    }
}
