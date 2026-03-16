using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvStationRepository(AppDbContext db) : IPdvStationRepository
{
    public async Task<IEnumerable<PdvStationEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.PdvStations
            .Where(s => s.CompanyId == companyId && s.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvStationEntity?> GetByIdAsync(int id, int companyId)
    {
        var model = await db.PdvStations
            .FirstOrDefaultAsync(s => s.Id == id && s.CompanyId == companyId && s.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<PdvStationEntity> CreateAsync(PdvStationEntity entity)
    {
        var model = new PdvStation
        {
            CompanyId = entity.CompanyId,
            Name = entity.Name,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvStations.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvStations.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvStation {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvStationEntity Map(PdvStation m) =>
        new PdvStationEntity().Init(m.Id, m.CompanyId, m.Name!, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}
