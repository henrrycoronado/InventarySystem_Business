using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvMenuItemRepository(AppDbContext db) : IPdvMenuItemRepository
{
    public async Task<IEnumerable<PdvMenuItemEntity>> GetAllByMenuAsync(int menuId)
    {
        var models = await db.PdvMenuItems
            .Where(i => i.MenuId == menuId && i.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvMenuItemEntity?> GetByIdAsync(int id)
    {
        var model = await db.PdvMenuItems
            .FirstOrDefaultAsync(i => i.Id == id && i.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<PdvMenuItemEntity> CreateAsync(PdvMenuItemEntity entity)
    {
        var model = new PdvMenuItem
        {
            MenuId = entity.MenuId,
            SkuId = entity.SkuId,
            StationId = entity.StationId,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvMenuItems.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvMenuItems.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvMenuItem {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvMenuItemEntity Map(PdvMenuItem m) =>
        new PdvMenuItemEntity().Init(m.Id, m.MenuId, m.SkuId, m.StationId, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}