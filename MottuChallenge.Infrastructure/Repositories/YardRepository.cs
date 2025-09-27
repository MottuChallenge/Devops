using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.DTOs.Response;
using MottuChallenge.Application.Pagination;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Infrastructure.Persistence;

namespace MottuChallenge.Infrastructure.Repositories
{
    internal class YardRepository(MottuChallengeContext context) : IYardRepository
    {
        private readonly MottuChallengeContext _context = context;

        public async Task<Yard> SaveYardAsync(Yard yard)
        {
           await _context.Yards.AddAsync(yard);
           await _context.SaveChangesAsync();
           return yard;
        }

        public async Task<Yard?> GetYardByIdAsync(Guid id)
        {
            return await _context.Yards
                .Include(y => y.Address)
                .Include(y => y.Points)
                .FirstOrDefaultAsync(y => y.Id == id);
        }

        public async Task<List<Yard>> GetAllYardsAsync()
        {
            return await _context.Yards
                .Include(y => y.Address)
                .Include(y => y.Points)
                .ToListAsync();
        }

        public async Task UpdateAsync(Yard yard)
        {
            _context.Yards.Update(yard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Yard yard)
        {
            _context.Yards.Remove(yard);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<YardResponseDto>> GetAllYardPaginated(
            PageRequest page,
            YardQuery? filter = null,
            CancellationToken ct = default)
        {
            page.EnsureValid();

            var query = _context.Yards
                .Include(y => y.Address)
                .Include(y => y.Points)
                .AsNoTracking();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                    query = query.Where(y => y.Name.Contains(filter.Name));
            }

            var totalItems = await query.CountAsync(ct);

            var yards = await query
                .Skip((page.Page - 1) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync(ct);

            var items = yards.Select(y => new YardResponseDto
            {
                Id = y.Id,
                Name = y.Name,
                Address = new AddressResponseDto
                {
                    Street = y.Address.Street,
                    Number = y.Address.Number,
                    Neighborhood = y.Address.Neighborhood,
                    City = y.Address.City,
                    State = y.Address.State,
                    ZipCode = y.Address.ZipCode,
                    Country = y.Address.Country
                },
                Points = y.Points.Select(p => new PointResponseDto
                {
                    PointOrder = p.PointOrder,
                    X = p.X,
                    Y = p.Y
                }).ToList()
            }).ToList();

            return new PaginatedResult<YardResponseDto>(items, totalItems, page.Page, page.PageSize);
        }

    }
}
