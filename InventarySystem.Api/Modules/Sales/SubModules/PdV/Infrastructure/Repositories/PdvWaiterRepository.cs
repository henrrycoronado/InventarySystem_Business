using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvWaiterRepository(AppDbContext db) : IPdvWaiterRepository
{
    public async Task<IEnumerable<PdvWaiterEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.PdvWaiters
            .Where(w => w.CompanyId == companyId && w.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvWaiterEntity?> GetByIdAsync(int id, int companyId)
    {
        var model = await db.PdvWaiters
            .FirstOrDefaultAsync(w => w.Id == id && w.CompanyId == companyId && w.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<PdvWaiterEntity> CreateAsync(PdvWaiterEntity entity)
    {
        var model = new PdvWaiter
        {
            CompanyId = entity.CompanyId,
            Name = entity.Name,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvWaiters.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvWaiters.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvWaiter {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvWaiterEntity Map(PdvWaiter m) =>
        new PdvWaiterEntity().Init(m.Id, m.CompanyId, m.Name!, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}