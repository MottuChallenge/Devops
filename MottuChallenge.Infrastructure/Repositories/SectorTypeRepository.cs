using Microsoft.EntityFrameworkCore;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Infrastructure.Persistence;

namespace MottuChallenge.Infrastructure.Repositories
{
    internal class SectorTypeRepository(MottuChallengeContext context) : ISectorTypeRepository
    {

        private readonly MottuChallengeContext _context = context;

        public async Task<SectorType> FindSectorByName(string name)
        {
            return await _context.SectorTypes
                        .FirstOrDefaultAsync(s => s.Name == name);

        }

        public async Task<SectorType> SaveSectorTypeAsync(SectorType sectorType)
        {
            await _context.SectorTypes.AddAsync(sectorType);
            await _context.SaveChangesAsync();
            return sectorType;
        }

        public async Task<List<SectorType>> GetAllSectorTypesAsync()
        {
            return await _context.SectorTypes.ToListAsync();
        }
        public async Task<SectorType> UpdateSectorTypeAsync(SectorType sectorType)
        {
            _context.SectorTypes.Update(sectorType);
            await _context.SaveChangesAsync();
            return sectorType;
        }

        public async Task<SectorType> FindByIdAsync(Guid id)
        {
            return await _context.SectorTypes.FindAsync(id);
        }

        public async Task DeleteSectorTypeAsync(SectorType sectorType)
        {
            _context.SectorTypes.Remove(sectorType);
            await _context.SaveChangesAsync();
        }



    }
}
