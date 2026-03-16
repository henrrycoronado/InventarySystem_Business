using InventarySystem.Api.Modules.Inventory.Domain.Entities;

namespace InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

public interface ICompanyAttributeRepository
{
    Task<IEnumerable<CompanyAttributeEntity>> GetAllByCompanyAsync(int companyId);
    Task<CompanyAttributeEntity?> GetByIdAsync(int id, int companyId);
    Task<CompanyAttributeEntity> CreateAsync(CompanyAttributeEntity entity);
    Task DeactivateAsync(int id);
}
