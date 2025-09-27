using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.UseCases.Yards
{
    public class UpdateYardUseCase
    {
        private readonly IYardRepository _yardRepository;

        public UpdateYardUseCase(IYardRepository yardRepository)
        {
            _yardRepository = yardRepository;
        }

        public async Task<Yard> UpdateYardNameAsync(Guid yardId, UpdateYardDto dto)
        {
            var yard = await _yardRepository.GetYardByIdAsync(yardId);
            if (yard == null)
                throw new KeyNotFoundException($"Yard with ID {yardId} not found.");

            yard.UpdateName(dto.Name);
            await _yardRepository.UpdateAsync(yard);

            return yard;
        }
    }
}
