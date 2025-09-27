using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.Yards
{
    public class DeleteYardUseCase
    {
        private readonly IYardRepository _yardRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public DeleteYardUseCase(IYardRepository yardRepository, IMotorcycleRepository motorcycleRepository)
        {
            _yardRepository = yardRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task DeleteYardAsync(Guid yardId)
        {
            var yard = await _yardRepository.GetYardByIdAsync(yardId);
            if (yard == null)
                throw new KeyNotFoundException($"Yard with ID {yardId} not found.");


            await _motorcycleRepository.RemoveMotorcyclesByYardId(yardId);
            await _yardRepository.DeleteAsync(yard);
        }
    }
}
