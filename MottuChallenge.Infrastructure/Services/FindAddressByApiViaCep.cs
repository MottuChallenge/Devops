using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Interfaces;
using MottuChallenge.Domain.Entities;
using System.Text.Json;

namespace MottuChallenge.Infrastructure.Services
{
    internal class FindAddressByApiViaCep : IAddressProvider
    {
        private readonly HttpClient _httpClient;

        public FindAddressByApiViaCep(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Address> GetAddressByCepAsync(string cep, string number)
        {
            var url = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await _httpClient.GetStringAsync(url);

            var viacepResponse = JsonSerializer.Deserialize<ViaCepResponse>(response)
                                 ?? throw new Exception("CEP não encontrado");

            return new Address(
                viacepResponse.Logradouro,
                int.Parse(number),
                viacepResponse.Bairro,
                viacepResponse.Localidade,
                viacepResponse.Uf,
                cep,
                "Brasil"
            );
        }
    }
}
