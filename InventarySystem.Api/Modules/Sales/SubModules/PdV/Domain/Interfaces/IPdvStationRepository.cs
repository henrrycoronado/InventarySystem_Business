using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvStationRepository
{
    Task<IEnumerable<PdvStationEntity>> GetAllByCompanyAsync(int companyId);
    Task<PdvStationEntity?> GetByIdAsync(int id, int companyId);
    Task<PdvStationEntity> CreateAsync(PdvStationEntity entity);
    Task DeactivateAsync(int id);
}
