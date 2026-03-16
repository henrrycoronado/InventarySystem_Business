using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Inventory.Infrastructure.Repositories;

public class BatchRepository(AppDbContext db) : IBatchRepository
{
    public async Task<IEnumerable<BatchEntity>> GetAllBySkuAsync(int skuId)
    {
        var models = await db.Batches
            .Where(b => b.SkuId == skuId && b.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<BatchEntity?> GetByIdAsync(int id)
    {
        var model = await db.Batches.FirstOrDefaultAsync(b => b.Id == id && b.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<BatchEntity> CreateAsync(BatchEntity entity)
    {
        var model = new Batch
        {
            SkuId = entity.SkuId,
            BatchNumber = entity.BatchNumber,
            ManufactureDate = entity.ManufactureDate,
            ExpirationDate = entity.ExpirationDate,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.Batches.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.Batches.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"Batch {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static BatchEntity Map(Batch m) =>
        new BatchEntity().Init(m.Id, m.SkuId, m.BatchNumber!, m.ManufactureDate, m.ExpirationDate, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}
