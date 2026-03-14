using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

public interface IPdvMenuItemRepository
{
    Task<IEnumerable<PdvMenuItemEntity>> GetAllByMenuAsync(int menuId);
    Task<PdvMenuItemEntity?> GetByIdAsync(int id);
    Task<PdvMenuItemEntity> CreateAsync(PdvMenuItemEntity entity);
    Task DeactivateAsync(int id);
}