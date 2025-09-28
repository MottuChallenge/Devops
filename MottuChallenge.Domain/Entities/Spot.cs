using MottuChallenge.Domain.Enums;
using MottuChallenge.Domain.Exceptions;
using MottuChallenge.Domain.Validations;
using System.Text.Json.Serialization;

namespace MottuChallenge.Domain.Entities
{
    public class Spot
    {
        public Guid SpotId { get; private set; }
        public Guid SectorId { get; private set; }
        [JsonIgnore]
        public Sector Sector { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public SpotStatus Status { get; private set; }
        public Motorcycle? Motorcycle { get; private set; }
        public Guid? MotorcycleId { get; set; }

        public Spot(double x, double y)
        {
            Guard.AgainstNegativeCoordinates(x, y, nameof(Spot));
            this.SpotId = Guid.NewGuid();
            this.X = x;
            this.Y = y;
            this.Status = SpotStatus.FREE;

        }
        private Spot() { }

        public void SetSector(Sector sector)
        {
            if (sector == null)
                throw new DomainValidationException("Sector cannot be null", nameof(sector), nameof(Spot));
            Sector = sector;
            SectorId = sector.Id;
        }

        public void AssignMotorcycle(Motorcycle motorcycle)
        {
            Motorcycle = motorcycle;
            MotorcycleId = motorcycle.Id;
            motorcycle.SetSpot(this);
            Status = SpotStatus.OCCUPIED;
        }

        public void RemoveMotorcycle()
        {
            Motorcycle = null;
            MotorcycleId = null;
            Status = SpotStatus.FREE;
        }
    }
}
