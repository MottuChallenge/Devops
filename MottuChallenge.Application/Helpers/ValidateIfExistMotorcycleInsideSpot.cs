using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.Helpers
{
    internal static class ValidateIfExistMotorcycleInsideSpot
    {
        public static void ValidateMotorcycleInsideSpot(Sector sector, Guid spotId)
        {
            if (sector == null)
            {
                throw new ArgumentNullException(nameof(sector), "Sector cannot be null.");
            }

            // Verifique se a coleção de Spots não é nula ou vazia
            if (sector.Spots == null || !sector.Spots.Any())
            {
                throw new InvalidOperationException("The sector does not contain any spots.");
            }

            var spot = sector.Spots.FirstOrDefault(s => s.SpotId == spotId);
            if (spot == null)
            {
                throw new KeyNotFoundException("Spot not found");
            }


            if (spot.Status == Domain.Enums.SpotStatus.OCCUPIED)
            {
                throw new ArgumentException("This Spot is already occupied");
            }
        }
    }
}
