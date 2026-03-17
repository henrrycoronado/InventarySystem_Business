using InventarySystem.Api.src.Core.Domain.Entities;
using InventarySystem.Api.src.Core.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.src.Core.Infrastructure.Repositories;

public class WarehouseRepository(AppDbContext db) : IWarehouseRepository
{
    public async Task<IEnumerable<WarehouseEntity>> GetAllByCompanyAsync(int companyId)
    {
        var warehouses = await db.Warehouses
            .Where(w => w.CompanyId == companyId && w.IsActive == true)
            .ToListAsync();

        var warehouseIds = warehouses.Select(w => w.Id).ToList();
        var stockTotals = await db.Stocks
            .Where(s => warehouseIds.Contains(s.WarehouseId) && s.IsActive == true)
            .GroupBy(s => s.WarehouseId)
            .Select(g => new { WarehouseId = g.Key, Total = g.Sum(s => s.Quantity - s.ReservedQuantity) })
            .ToListAsync();

        return warehouses.Select(w =>
        {
            var total = stockTotals.FirstOrDefault(t => t.WarehouseId == w.Id)?.Total ?? 0;
            return new WarehouseEntity().Init(w.Id, w.CompanyId, w.Name!, w.IsActive ?? true, w.CreatedAt ?? DateTime.Now)
                .WithTotalStock(total);
        });
    }

    public async Task<WarehouseEntity?> GetByIdAsync(int id, int companyId)
    {
        var w = await db.Warehouses
            .FirstOrDefaultAsync(w => w.Id == id && w.CompanyId == companyId && w.IsActive == true);
        if (w is null) return null;

        var total = await db.Stocks
            .Where(s => s.WarehouseId == id && s.IsActive == true)
            .SumAsync(s => s.Quantity - s.ReservedQuantity);

        return new WarehouseEntity().Init(w.Id, w.CompanyId, w.Name!, w.IsActive ?? true, w.CreatedAt ?? DateTime.Now)
            .WithTotalStock(total);
    }

    public async Task<WarehouseEntity> CreateAsync(WarehouseEntity entity)
    {
        var model = new Warehouse
        {
            CompanyId = entity.CompanyId,
            Name = entity.Name,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.Warehouses.Add(model);
        await db.SaveChangesAsync();
        return new WarehouseEntity().Init(model.Id, model.CompanyId, model.Name!, model.IsActive ?? true, model.CreatedAt ?? DateTime.Now);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.Warehouses.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"Warehouse {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }
}