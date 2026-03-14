using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;
using InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Infrastructure.Repositories;

public class PdvOrderDetailRepository(AppDbContext db) : IPdvOrderDetailRepository
{
    public async Task<IEnumerable<PdvOrderDetailEntity>> GetAllByOrderAsync(int orderId)
    {
        var models = await db.PdvOrderDetails
            .Where(d => d.OrderId == orderId && d.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<PdvOrderDetailEntity> CreateAsync(PdvOrderDetailEntity entity)
    {
        var model = new PdvOrderDetail
        {
            OrderId = entity.OrderId,
            MenuItemId = entity.MenuItemId,
            StationId = entity.StationId,
            StatusId = entity.StatusId,
            Quantity = entity.Quantity,
            UnitPrice = entity.UnitPrice,
            Notes = entity.Notes,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.PdvOrderDetails.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task UpdateStatusAsync(int id, int statusId)
    {
        var model = await db.PdvOrderDetails.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvOrderDetail {id} not found");
        model.StatusId = statusId;
        await db.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.PdvOrderDetails.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"PdvOrderDetail {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static PdvOrderDetailEntity Map(PdvOrderDetail m) =>
        new PdvOrderDetailEntity().Init(m.Id, m.OrderId, m.MenuItemId, m.StationId, m.StatusId, m.Quantity, m.UnitPrice, m.Notes, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}