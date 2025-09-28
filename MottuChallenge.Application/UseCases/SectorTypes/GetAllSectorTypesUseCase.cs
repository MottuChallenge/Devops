using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.SectorTypes
{
    public class GetAllSectorTypesUseCase
    {
        private readonly ISectorTypeRepository _repository;

        public GetAllSectorTypesUseCase(ISectorTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SectorTypeResponseDto>> FindAllSectorTypes()
        {
            var sectorTypes = await _repository.GetAllSectorTypesAsync();
            return sectorTypes.Select(st => new SectorTypeResponseDto
            {
                Id = st.Id,
                Name = st.Name
            }).ToList();
        }
    }
}
