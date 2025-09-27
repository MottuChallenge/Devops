using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.SectorTypes
{
    public class UpdateSectorTypeUseCase
    {
        private readonly ISectorTypeRepository _repository;

        public UpdateSectorTypeUseCase(ISectorTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<SectorTypeResponseDto> UpdateSectorTypeById(SectorTypeDto dto, Guid id)
        {
            var sectorType = await _repository.FindByIdAsync(id);
            if (sectorType == null)
                throw new KeyNotFoundException($"SectorType with id {id} not found.");

            sectorType.AlterName(dto.Name);
            await _repository.UpdateSectorTypeAsync(sectorType);

            return new SectorTypeResponseDto
            {
                Id = sectorType.Id,
                Name = sectorType.Name
            };
        }
    }
}
