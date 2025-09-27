using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Infrastructure.Persistence;

namespace MottuChallenge.Infrastructure.Repositories
{
    internal class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly MottuChallengeContext _context;

        public MotorcycleRepository(MottuChallengeContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Remove(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<MotorcycleResponseDto>> GetAllMotorciclePaginated(
                 PageRequest page,
                 MotorcycleQuery? filter = null,
                 CancellationToken ct = default
        )
        {
            page.EnsureValid();

            var query = _context.Motorcycles.AsNoTracking();

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Plate))
                    query = query.Where(m => m.Plate.Contains(filter.Plate));
            }

            var totalItems = await query.CountAsync(ct);

            var motorcycles = await query
                .Skip((page.Page - 1) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync(ct);

            var items = motorcycles.Select(m => new MotorcycleResponseDto
            {
                Id = m.Id,
                Model = m.Model,
                EngineType = m.EngineType,
                Plate = m.Plate,
                LastRevisionDate = m.LastRevisionDate,
                SpotId = m.SpotId
            }).ToList();

            return new PaginatedResult<MotorcycleResponseDto>(items, totalItems, page.Page, page.PageSize);
        }

        public async Task<Motorcycle> GetByIdAsync(Guid id)
        {
            return await _context.Motorcycles.FindAsync(id);
        }

        public async Task<Motorcycle> SaveMotorcycleAsync(Motorcycle motorcycle)
        {
            await _context.Motorcycles.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
            return motorcycle;
        }

        public async Task<Motorcycle> UpdateAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
            await _context.SaveChangesAsync();
            return motorcycle;
        }

        public async Task RemoveMotorcyclesByYardId(Guid yardId)
        {
            var motorcycles = await _context.Motorcycles
                .Where(m => m.Spot.Sector.YardId == yardId)
                .ToListAsync();

            if (motorcycles.Any())
            {
                _context.Motorcycles.RemoveRange(motorcycles);

                await _context.SaveChangesAsync();
            }
        }

    }
}
