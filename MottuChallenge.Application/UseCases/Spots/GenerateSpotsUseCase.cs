using MottuChallenge.Application.Helpers;
using MottuChallenge.Domain.Entities;


namespace MottuChallenge.Application.UseCases.Spots
{
    public class GenerateSpotsUseCase
    {
        public List<Spot> GenerateSpot(Sector sector, double width, double height)
        {
            var spots = new List<Spot>();

            double minX = sector.Points.Min(p => p.X);
            double maxX = sector.Points.Max(p => p.X);
            double minY = sector.Points.Min(p => p.Y);
            double maxY = sector.Points.Max(p => p.Y);

            double y = minY;

            while (y <= maxY)
            {
                double x = minX;
                while (x <= maxX)
                {
                    if (GeometryHelper.IsPointInsidePolygon(x, y, sector.Points.ToList()))
                    {
                        var spot = new Spot(x, y);
                        spot.SetSector(sector);
                        spots.Add(spot);
                    }
                    x += width;
                }
                y += height;
            }

            return spots;
        }
    }
}
