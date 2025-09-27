using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.ValueObjects;

namespace MottuChallenge.Application.Helpers
{
    internal static class PolygonPointsMapping
    {
        public static List<PointResponseDto> CreateListOfPointResponseDto(IEnumerable<PolygonPoint> polygonPoint)
        {
            var points = new List<PointResponseDto>();

            foreach (var point in polygonPoint)
            {
                points.Add(new PointResponseDto { PointOrder = point.PointOrder, X = point.X, Y = point.Y });
            }

            return points;
        }
    }
}
