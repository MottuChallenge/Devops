using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.UseCases.SectorTypes
{
    public class CreateSectorTypeUseCase
    {
        private readonly ISectorTypeRepository _repository;

        public CreateSectorTypeUseCase(ISectorTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<SectorType> SaveSectorType(SectorTypeDto dto)
        {
            var existing = await _repository.FindSectorByName(dto.Name.ToLower());
            if (existing != null)
                throw new Exception("Já existe SectorType com esse nome");

            var sectorType = new SectorType(dto.Name.ToLower());
            await _repository.SaveSectorTypeAsync(sectorType);
            return sectorType;
        }
    }
}
