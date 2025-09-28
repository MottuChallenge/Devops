using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Helpers;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Application.Repositories;

namespace MottuChallenge.Application.UseCases.Yards
{
    public class GetYardByIdUseCase
    {
        private readonly IYardRepository _yardRepository;

        public GetYardByIdUseCase(IYardRepository yardRepository)
        {
            _yardRepository = yardRepository;
        }

        public async Task<YardResponseDto> FindYardById(Guid id)
        {
            var yard = await _yardRepository.GetYardByIdAsync(id);
            ValidateYardExists(yard);

            return new YardResponseDto
            {
                Id = yard.Id,
                Name = yard.Name,
                Address = AddressMapping.CreateAddressResponseDto(yard.Address),
                Points = PolygonPointsMapping.CreateListOfPointResponseDto(yard.Points)
            };
        }

        private void ValidateYardExists(Yard yard)
        {
            if (yard == null)
            {
                throw new KeyNotFoundException("Yard not Found");
            }
        }
    }
}
