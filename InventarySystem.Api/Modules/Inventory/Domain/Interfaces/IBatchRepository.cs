using InventarySystem.Api.Modules.Inventory.Domain.Entities;

namespace InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

public interface IBatchRepository
{
    Task<IEnumerable<BatchEntity>> GetAllBySkuAsync(int skuId);
    Task<BatchEntity?> GetByIdAsync(int id);
    Task<BatchEntity> CreateAsync(BatchEntity entity);
    Task DeactivateAsync(int id);
}
