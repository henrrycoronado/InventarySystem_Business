using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.DTOs;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Interfaces;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Application.Services;

public class PdvMenuItemService(IPdvMenuItemRepository repository) : IPdvMenuItemService
{
    public async Task<IEnumerable<PdvMenuItemDto>> GetAllByMenuAsync(int menuId)
    {
        var items = await repository.GetAllByMenuAsync(menuId);
        return items.Select(Map);
    }

    public async Task<PdvMenuItemDto?> GetByIdAsync(int id)
    {
        var item = await repository.GetByIdAsync(id);
        return item is null ? null : Map(item);
    }

    public async Task<PdvMenuItemDto> CreateAsync(PdvMenuItemCreateDto dto)
    {
        var entity = PdvMenuItemEntity.Create(dto.MenuId, dto.SkuId, dto.StationId);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task DeactivateAsync(int id) => await repository.DeactivateAsync(id);

    private static PdvMenuItemDto Map(PdvMenuItemEntity e) => new()
    {
        Id = e.Id, MenuId = e.MenuId, SkuId = e.SkuId,
        StationId = e.StationId, CreatedAt = e.CreatedAt
    };
}