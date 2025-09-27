using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.Interfaces
{
    public interface IAddressProvider
    {
        Task<Address> GetAddressByCepAsync(string cep, string number);
    }
}
