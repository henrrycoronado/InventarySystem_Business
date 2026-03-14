using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvTableRepository
{
    Task<IEnumerable<PdvTableEntity>> GetAllByCompanyAsync(int companyId);
    Task<PdvTableEntity?> GetByIdAsync(int id, int companyId);
    Task<PdvTableEntity> CreateAsync(PdvTableEntity entity);
    Task DeactivateAsync(int id);
}