using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.Interfaces;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.ValueObjects;

namespace MottuChallenge.Application.UseCases.Yards
{
    public class CreateYardUseCase
    {
        private readonly IYardRepository _yardRepository;
        private readonly IAddressProvider _addressProvider;

        public CreateYardUseCase(IYardRepository yardRepository, IAddressProvider addressProvider)
        {
            _yardRepository = yardRepository;
            _addressProvider = addressProvider;
        }

        public async Task<Yard> SaveYard(CreateYardDto dto)
        {
            var address = await _addressProvider.GetAddressByCepAsync(dto.Cep, dto.Number);
            if (address is null)
                throw new ArgumentException($"Endereço não encontrado para o CEP {dto.Cep}");

            var yard = new Yard(dto.Name);
            yard.SetAddress(address);

            yard.AddPoints(dto.Points.Select(p => new PolygonPoint(p.PointOrder, p.X, p.Y)));

            return await _yardRepository.SaveYardAsync(yard);
        }
    }
}
