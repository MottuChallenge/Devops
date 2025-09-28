using MottuChallenge.Domain.ValueObjects;

namespace MottuChallenge.Application.Helpers
{
    public static class GeometryHelper
    {
        public static bool IsPointInsidePolygon(double x, double y, List<PolygonPoint> polygon)
        {
            bool inside = false;
            int n = polygon.Count;

            for (int i = 0; i < n; i++)
            {
                var p1 = polygon[i];
                var p2 = polygon[(i + 1) % n];

                if ((p1.Y > y) != (p2.Y > y) &&
                    x < (p2.X - p1.X) * (y - p1.Y) / (p2.Y - p1.Y) + p1.X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
}
