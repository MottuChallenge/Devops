using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Application.Repositories
{
    public interface ISectorTypeRepository
    {

        Task<SectorType> SaveSectorTypeAsync(SectorType sectorType);

        Task<SectorType> FindSectorByName(string name);

        Task<List<SectorType>> GetAllSectorTypesAsync();

        Task<SectorType> UpdateSectorTypeAsync(SectorType sectorType);

        Task<SectorType> FindByIdAsync(Guid id);

        Task DeleteSectorTypeAsync(SectorType sectorType);
    }
}
