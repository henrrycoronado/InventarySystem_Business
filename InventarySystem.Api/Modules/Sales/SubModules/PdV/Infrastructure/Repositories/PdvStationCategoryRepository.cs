using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvStationCategoryRepository(AppDbContext db) : IPdvStationCategoryRepository
{
    public async Task<IEnumerable<PdvStationCategoryEntity>> GetAllByStationAsync(int stationId)
    {
        var models = await db.PdvStationCategories
            .Where(sc => sc.StationId == stationId && sc.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvStationCategoryEntity> CreateAsync(PdvStationCategoryEntity entity)
    {
        var model = new PdvStationCategory
        {
            StationId = entity.StationId,
            GlobalCategoryId = entity.GlobalCategoryId,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvStationCategories.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvStationCategories.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvStationCategory {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvStationCategoryEntity Map(PdvStationCategory m) =>
        new PdvStationCategoryEntity().Init(m.Id, m.StationId, m.GlobalCategoryId, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}
