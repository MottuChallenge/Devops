using MottuChallenge.Application.Helpers;
using MottuChallenge.Domain.Exceptions;
using MottuChallenge.Domain.Validations;
using MottuChallenge.Domain.ValueObjects;

namespace MottuChallenge.Domain.Entities
{
    public class Sector
    {
        public Guid Id { get; private set; }
        public Guid YardId { get; private set; }
        public Yard Yard { get; private set; }
        public SectorType SectorType { get; private set; }
        public Guid SectorTypeId { get; private set; }

        private readonly List<PolygonPoint> _points = new();
        public IReadOnlyCollection<PolygonPoint> Points => _points.AsReadOnly();

        private readonly List<Spot> _spots = new();
        public IReadOnlyCollection<Spot> Spots => _spots.AsReadOnly();

        public Sector()
        {
            this.Id = Guid.NewGuid();
        }

        public void AddSectorType(SectorType sectorType)
        {
            if (sectorType == null)
                throw new DomainValidationException("SectorType not be null", nameof(sectorType), nameof(Sector));
            SectorType = sectorType;
            SectorTypeId = sectorType.Id;
        }

        public void AddYard(Yard yard)
        {
            if (yard == null)
                throw new DomainValidationException("Yard not be null", nameof(yard), nameof(Sector));

            Yard = yard;
            YardId = yard.Id;
        }

        public void AddSpots(IEnumerable<Spot> spots)
        {
            _spots.AddRange(spots);
        }
        public void AddPoints(IEnumerable<PolygonPoint> points)
        {
            if (points.Count() < 3)
                throw new DomainValidationException("O setor deve ter pelo menos 3 pontos", nameof(points), nameof(Sector));
            _points.AddRange(points);
        }

        public void AssignMotorcycleToSpot(Guid spotId, Motorcycle motorcycle)
        {
            var spot = _spots.FirstOrDefault(s => s.SpotId == spotId);
            if (spot == null)
                throw new KeyNotFoundException("Spot not found in this sector.");

            spot.AssignMotorcycle(motorcycle);
        }

        public void ValidateInsideYard()
        {
            var points = Yard.Points.OrderBy(p => p.PointOrder).ToList();
            bool isInside = _points.All(p => GeometryHelper.IsPointInsidePolygon(p.X, p.Y, points));
            if (!isInside)
                throw new DomainValidationException("O setor deve estar completamente dentro do pátio", nameof(_points), nameof(Sector));
        }

        public void ValidateOverlap(IEnumerable<Sector> existingSectors)
        {
            if (!existingSectors.Any())
                return;

            foreach (var existing in existingSectors)
            {
                bool overlap = _points.Any(p => GeometryHelper.IsPointInsidePolygon(p.X, p.Y, existing.Points.ToList()));
                if (overlap)
                    throw new DomainValidationException("O setor não deve sobrepor outros setores no mesmo pátio", nameof(_points), nameof(Sector));
            }
        }
    }
}
