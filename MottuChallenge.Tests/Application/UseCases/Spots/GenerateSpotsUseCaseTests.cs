using MottuChallenge.Application.Helpers;
using MottuChallenge.Application.UseCases.Spots;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.ValueObjects;

namespace MottuChallenge.Test.Application.UseCases.Spots;

public class GenerateSpotsUseCaseTests
{
    private readonly GenerateSpotsUseCase _useCase = new GenerateSpotsUseCase();

    private Sector CreateRectangleSector(double minX, double minY, double maxX, double maxY)
    {
        var sector = new Sector();
        sector.AddPoints(new List<PolygonPoint>
        {
            new(0, minX, minY),
            new(1, maxX, minY),
            new(2, maxX, maxY),
            new(3, minX, maxY)
        });
        return sector;
        
    }

    [Fact]
    public void GenerateSpot_ReturnsExpectedCount_ForRectangle()
    {
        var sector = CreateRectangleSector(0, 0, 2, 2);
        const double width = 0.9;
        const double height = 0.9;

        var spots = _useCase.GenerateSpot(sector, width, height);

        Assert.Equal(9, spots.Count);
    }

    [Fact]
    public void GenerateSpot_AssignsSector_ToEachSpot()
    {
        var sector = CreateRectangleSector(0, 0, 1, 1);
        var spots = _useCase.GenerateSpot(sector, 0.6, 0.6);

        Assert.All(spots, s => Assert.Equal(sector, s.Sector));
    }

    [Fact]
    public void GenerateSpot_PointsAreInsidePolygon()
    {
        var sector = CreateRectangleSector(0, 0, 2, 2);
        var spots = _useCase.GenerateSpot(sector, 0.9, 0.9);

        Assert.All(spots, s => Assert.True(GeometryHelper.IsPointInsidePolygon(s.X, s.Y, sector.Points.ToList())));
    }
    
    [Fact]
    public void GenerateSpot_CreatesExpectedQuantity_BasedOnGridCalculation()
    {
        var sector = CreateRectangleSector(0, 0, 2, 2);
        const double width = 0.9;
        const double height = 0.9;

        double minX = double.MaxValue, maxX = double.MinValue, minY = double.MaxValue, maxY = double.MinValue;
        foreach (var p in sector.Points)
        {
            if (p.X < minX) minX = p.X;
            if (p.X > maxX) maxX = p.X;
            if (p.Y < minY) minY = p.Y;
            if (p.Y > maxY) maxY = p.Y;
        }

        var cols = (int)Math.Floor((maxX - minX) / width) + 1;
        var rows = (int)Math.Floor((maxY - minY) / height) + 1;
        var expected = cols * rows;

        var spots = _useCase.GenerateSpot(sector, width, height);

        Assert.Equal(expected, spots.Count);
    }
}