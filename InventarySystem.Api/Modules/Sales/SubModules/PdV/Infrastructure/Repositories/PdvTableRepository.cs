using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvTableRepository(AppDbContext db) : IPdvTableRepository
{
    public async Task<IEnumerable<PdvTableEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.PdvTables
            .Where(t => t.CompanyId == companyId && t.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvTableEntity?> GetByIdAsync(int id, int companyId)
    {
        var model = await db.PdvTables
            .FirstOrDefaultAsync(t => t.Id == id && t.CompanyId == companyId && t.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<PdvTableEntity> CreateAsync(PdvTableEntity entity)
    {
        var model = new PdvTable
        {
            CompanyId = entity.CompanyId,
            Name = entity.Name,
            Capacity = entity.Capacity,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvTables.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvTables.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvTable {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvTableEntity Map(PdvTable m) =>
        new PdvTableEntity().Init(m.Id, m.CompanyId, m.Name!, m.Capacity, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}