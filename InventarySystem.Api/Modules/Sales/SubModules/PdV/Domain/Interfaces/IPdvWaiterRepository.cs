using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvWaiterRepository
{
    Task<IEnumerable<PdvWaiterEntity>> GetAllByCompanyAsync(int companyId);
    Task<PdvWaiterEntity?> GetByIdAsync(int id, int companyId);
    Task<PdvWaiterEntity> CreateAsync(PdvWaiterEntity entity);
    Task DeactivateAsync(int id);
}