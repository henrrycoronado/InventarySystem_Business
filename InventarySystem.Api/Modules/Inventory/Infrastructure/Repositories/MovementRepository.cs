using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Inventory.Infrastructure.Repositories;

public class MovementRepository(AppDbContext db) : IMovementRepository
{
    public async Task<IEnumerable<MovementEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.Movements
            .Include(m => m.MovementDetails)
                .ThenInclude(d => d.Sku)
                    .ThenInclude(s => s!.CompanyProduct)
                        .ThenInclude(cp => cp!.GlobalProduct)
            .Where(m => m.CompanyId == companyId && m.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<MovementEntity?> GetByIdAsync(int id)
    {
        var model = await db.Movements
            .Include(m => m.MovementDetails)
                .ThenInclude(d => d.Sku)
                    .ThenInclude(s => s!.CompanyProduct)
                        .ThenInclude(cp => cp!.GlobalProduct)
            .FirstOrDefaultAsync(m => m.Id == id && m.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<MovementEntity> CreateAsync(MovementEntity entity)
    {
        var model = new Movement
        {
            CompanyId = entity.CompanyId,
            WarehouseId = entity.WarehouseId,
            TargetWarehouseId = entity.TargetWarehouseId,
            StatusId = entity.StatusId,
            TypeId = entity.TypeId,
            Notes = entity.Notes,
            MovementDate = entity.MovementDate,
            IsActive = entity.IsActive,
            CreatedAt = DateTime.Now
        };
        db.Movements.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.Movements.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"Movement {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static MovementEntity Map(Movement m) =>
        new MovementEntity().Init(m.Id, m.CompanyId, m.WarehouseId, m.TargetWarehouseId, m.StatusId, m.TypeId, m.MovementDate ?? DateTime.Now, m.Notes, m.IsActive ?? true)
            .WithDetails(m.MovementDetails.Select(d => new MovementDetailExpandedDto
            {
                Id = d.Id,
                SkuId = d.SkuId,
                BatchId = d.BatchId,
                Quantity = d.Quantity,
                UnitCost = d.UnitCost,
                Sku = d.Sku is null ? null : new()
                {
                    Id = d.Sku.Id,
                    InternalSku = d.Sku.InternalSku!,
                    RetailPrice = d.Sku.RetailPrice ?? 0,
                    CompanyProductId = d.Sku.CompanyProductId,
                    CompanyProduct = d.Sku.CompanyProduct is null ? null : new()
                    {
                        Id = d.Sku.CompanyProduct.Id,
                        LocalNameAlias = d.Sku.CompanyProduct.LocalNameAlias,
                        WholesalePrice = d.Sku.CompanyProduct.WholesalePrice ?? 0,
                        GlobalProduct = d.Sku.CompanyProduct.GlobalProduct is null ? null : new()
                        {
                            Id = d.Sku.CompanyProduct.GlobalProduct.Id,
                            Name = d.Sku.CompanyProduct.GlobalProduct.Name!,
                            Brand = d.Sku.CompanyProduct.GlobalProduct.Brand,
                            UpcBarcode = d.Sku.CompanyProduct.GlobalProduct.UpcBarcode
                        }
                    }
                }
            }));
}