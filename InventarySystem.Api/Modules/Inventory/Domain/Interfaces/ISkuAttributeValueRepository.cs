using InventarySystem.Api.Modules.Inventory.Domain.Entities;

namespace InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

public interface ISkuAttributeValueRepository
{
    Task<IEnumerable<SkuAttributeValueEntity>> GetAllBySkuAsync(int skuId);
    Task<SkuAttributeValueEntity> CreateAsync(SkuAttributeValueEntity entity);
    Task DeactivateAsync(int id);
}
