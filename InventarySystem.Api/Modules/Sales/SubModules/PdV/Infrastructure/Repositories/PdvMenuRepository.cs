using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvMenuRepository(AppDbContext db) : IPdvMenuRepository
{
    public async Task<IEnumerable<PdvMenuEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.PdvMenus
            .Where(m => m.CompanyId == companyId && m.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvMenuEntity?> GetByIdAsync(int id, int companyId)
    {
        var model = await db.PdvMenus
            .FirstOrDefaultAsync(m => m.Id == id && m.CompanyId == companyId && m.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<PdvMenuEntity> CreateAsync(PdvMenuEntity entity)
    {
        var model = new PdvMenu
        {
            CompanyId = entity.CompanyId,
            Name = entity.Name,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvMenus.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvMenus.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvMenu {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvMenuEntity Map(PdvMenu m) =>
        new PdvMenuEntity().Init(m.Id, m.CompanyId, m.Name!, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}