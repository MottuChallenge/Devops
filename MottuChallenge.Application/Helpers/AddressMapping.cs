using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.Helpers
{
    internal static class AddressMapping
    {
        public static AddressResponseDto CreateAddressResponseDto(Address address)
        {
            return new AddressResponseDto { City = address.City, Country = address.Country, Neighborhood = address.Neighborhood, Number = address.Number, State = address.State, Street = address.Street, ZipCode = address.ZipCode };
        }
    }
}
