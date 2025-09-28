using MottuChallenge.Domain.Exceptions;
using MottuChallenge.Domain.Validations;

namespace MottuChallenge.Domain.ValueObjects
{
    public class PolygonPoint
    {
        public int PointOrder { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
       

        public PolygonPoint(int pointOrder, double x, double y)
        {
            ValidatePointOrder(pointOrder);
            Guard.AgainstNegativeCoordinates(x, y, nameof(PolygonPoint));   
            PointOrder = pointOrder;
            X = x;
            Y = y;
        }

        private PolygonPoint() { }

        private void ValidatePointOrder(int pointOrder)
        {
            if (pointOrder < 0)
            {
                throw new DomainValidationException("Point order must be non-negative", nameof(pointOrder), nameof(PolygonPoint));
            }
        }
    
    }
}
