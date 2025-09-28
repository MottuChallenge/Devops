using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Infrastructure.Persistence;

namespace MottuChallenge.Infrastructure.Repositories
{
    internal class SectorRepository(MottuChallengeContext context) : ISectorRepository
    {

        private readonly MottuChallengeContext _context = context;

        public async Task<Sector> SaveSectorAsync(Sector sector)
        {
            await _context.Sectors.AddAsync(sector);
            await _context.SaveChangesAsync();
            return sector;
        }

        public async Task<List<Sector>> GetSectorsByYardIdAsync(Guid yardId)
        {
            return await _context.Sectors
                                 .Where(s => s.YardId == yardId)
                                 .ToListAsync();
        }

        public async Task<Sector> GetSectorByIdAsync(Guid sectorId)
        {
            return await _context.Sectors
                                 .Include(s => s.Spots)
                                 .FirstOrDefaultAsync(s => s.Id == sectorId);
        }

        public async Task<List<Sector>> GetAllSectorsAsync()
        {
            return await _context.Sectors
                                 .Include(s => s.Spots)
                                 .ToListAsync();
        }

        public async Task<Sector> GetSectorBySpotId(Guid spotId)
        {
            return await _context.Sectors
                                 .Include(s => s.Spots)
                                 .FirstOrDefaultAsync(s => s.Spots.Any(sp => sp.SpotId == spotId));
        }

        public async Task<Sector> UpdateAsync(Sector sector)
        {
            _context.Sectors.Update(sector);
            await _context.SaveChangesAsync();
            return sector;

        }

        public async Task DeleteAsync(Sector sector)
        {
            _context.Sectors.Remove(sector);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<SectorResponseDto>> GetAllSectorPaginated(
            PageRequest page,
            SectorQuery? filter = null,
            CancellationToken ct = default)
        {
            page.EnsureValid();

            var query = _context.Sectors
                .Include(s => s.Points)
                .Include(s => s.Spots)
                .AsNoTracking();

            if (filter != null)
            {
                if (filter.YardId != Guid.Empty)
                    query = query.Where(s => s.YardId == filter.YardId);

                if (filter.SectorTypeId != Guid.Empty)
                    query = query.Where(s => s.SectorTypeId == filter.SectorTypeId);
            }

            var totalItems = await query.CountAsync(ct);

            var sectors = await query
                .Skip((page.Page - 1) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync(ct);

            var items = sectors.Select(s => new SectorResponseDto
            {
                Id = s.Id,
                YardId = s.YardId,
                SectorTypeId = s.SectorTypeId,
                Points = s.Points.Select(p => new PointResponseDto
                {
                    PointOrder = p.PointOrder,
                    X = p.X,
                    Y = p.Y
                }).ToList(),
                Spots = s.Spots.Select(sp => new SpotResponseDto
                {
                    SpotId = sp.SpotId,
                    SectorId = sp.SectorId,
                    Status = sp.Status,
                    MotorcycleId = sp.MotorcycleId,
                    X = sp.X,
                    Y = sp.Y
                }).ToList()
            }).ToList();

            return new PaginatedResult<SectorResponseDto>(items, totalItems, page.Page, page.PageSize);
        }

    }
}
