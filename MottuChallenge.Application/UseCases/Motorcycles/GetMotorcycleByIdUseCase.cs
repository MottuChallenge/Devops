using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.Motorcycles
{
    public class GetMotorcycleByIdUseCase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public GetMotorcycleByIdUseCase(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<MotorcycleResponseDto> FindMotorcycleById(Guid id)
        {
            var motorcycle =  await _motorcycleRepository.GetByIdAsync(id);

            var motorcycleDto = new MotorcycleResponseDto()
            {
                Id = motorcycle.Id,
                Model = motorcycle.Model,
                EngineType = motorcycle.EngineType,
                Plate = motorcycle.Plate,
                LastRevisionDate = motorcycle.LastRevisionDate,
                SpotId = motorcycle.SpotId
            };

            return motorcycleDto;
        } 
    }
}
