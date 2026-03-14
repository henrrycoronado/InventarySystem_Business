using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvMenuRepository
{
    Task<IEnumerable<PdvMenuEntity>> GetAllByCompanyAsync(int companyId);
    Task<PdvMenuEntity?> GetByIdAsync(int id, int companyId);
    Task<PdvMenuEntity> CreateAsync(PdvMenuEntity entity);
    Task DeactivateAsync(int id);
}