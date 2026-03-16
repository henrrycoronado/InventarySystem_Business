using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvStationCategoryRepository
{
    Task<IEnumerable<PdvStationCategoryEntity>> GetAllByStationAsync(int stationId);
    Task<PdvStationCategoryEntity> CreateAsync(PdvStationCategoryEntity entity);
    Task DeactivateAsync(int id);
}
