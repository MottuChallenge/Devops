using MottuChallenge.Application.DTOs.Request;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Application.UseCases.Spots;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.ValueObjects;

public class CreateSectorUseCase
{
    private readonly ISectorRepository _sectorRepository;
    private readonly IYardRepository _yardRepository;
    private readonly ISectorTypeRepository _sectorTypeRepository;
    private readonly GenerateSpotsUseCase _generateSpotsUseCase;

    public CreateSectorUseCase(
        ISectorRepository sectorRepository,
        IYardRepository yardRepository,
        ISectorTypeRepository sectorTypeRepository,
        GenerateSpotsUseCase generateSpotsUseCase)
    {
        _sectorRepository = sectorRepository;
        _yardRepository = yardRepository;
        _sectorTypeRepository = sectorTypeRepository;
        _generateSpotsUseCase = generateSpotsUseCase;
    }

    public async Task<Sector> SaveSector(SectorCreateDto sectorCreateDto)
    {
        var sector = new Sector();

        var yard = await _yardRepository.GetYardByIdAsync(sectorCreateDto.YardId)
                  ?? throw new KeyNotFoundException("Yard not found");

        var sectorType = await _sectorTypeRepository.FindByIdAsync(sectorCreateDto.SectorTypeId)
                        ?? throw new KeyNotFoundException("SectorType not found");

        sector.AddYard(yard);
        sector.AddSectorType(sectorType);

        sector.AddPoints(sectorCreateDto.Points.Select(p => new PolygonPoint(p.PointOrder, p.X, p.Y)));

        sector.ValidateInsideYard();

        var existingSectors = await _sectorRepository.GetSectorsByYardIdAsync(yard.Id);
        sector.ValidateOverlap(existingSectors);

        var spots = _generateSpotsUseCase.GenerateSpot(sector, 2, 2);
        sector.AddSpots(spots);

        return await _sectorRepository.SaveSectorAsync(sector);
    }
}
