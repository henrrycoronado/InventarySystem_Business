using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvOrderRepository(AppDbContext db) : IPdvOrderRepository
{
    public async Task<IEnumerable<PdvOrderEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.PdvOrders
            .Where(o => o.CompanyId == companyId && o.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvOrderEntity?> GetByIdAsync(int id)
    {
        var model = await db.PdvOrders
            .FirstOrDefaultAsync(o => o.Id == id && o.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<PdvOrderEntity> CreateAsync(PdvOrderEntity entity)
    {
        var model = new PdvOrder
        {
            CompanyId = entity.CompanyId,
            WarehouseId = entity.WarehouseId,
            TableId = entity.TableId,
            WaiterId = entity.WaiterId,
            StatusId = entity.StatusId,
            CustomerId = entity.CustomerId,
            OpenedAt = entity.OpenedAt,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvOrders.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task UpdateStatusAsync(int id, int statusId)
    {
        var model = await db.PdvOrders.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvOrder {id} not found");
        model.StatusId = statusId;
        await db.SaveChangesAsync();
    }

    public async Task CloseAsync(int id, int saleId)
    {
        var model = await db.PdvOrders.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvOrder {id} not found");
        model.SaleId = saleId;
        model.ClosedAt = DateTime.Now;
        await db.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvOrders.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvOrder {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvOrderEntity Map(PdvOrder m) =>
        new PdvOrderEntity().Init(m.Id, m.CompanyId, m.WarehouseId, m.TableId, m.WaiterId, m.StatusId, m.CustomerId, m.SaleId, m.OpenedAt ?? DateTime.Now, m.ClosedAt, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}