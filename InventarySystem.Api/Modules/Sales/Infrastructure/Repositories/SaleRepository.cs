using InventarySystem.Api.Modules.Sales.Application.DTOs;
using InventarySystem.Api.Modules.Sales.Domain.Entities;
using InventarySystem.Api.Modules.Sales.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Sales.Infrastructure.Repositories;

public class SaleRepository(AppDbContext db) : ISaleRepository
{
    public async Task<IEnumerable<SaleEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.Sales
            .Include(s => s.Status)
            .Include(s => s.Customer)
            .Include(s => s.Seller)
            .Include(s => s.SaleDetails)
                .ThenInclude(d => d.Sku)
                    .ThenInclude(sk => sk!.CompanyProduct)
                        .ThenInclude(cp => cp!.GlobalProduct)
            .Where(s => s.CompanyId == companyId && s.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<SaleEntity?> GetByIdAsync(int id)
    {
        var model = await db.Sales
            .Include(s => s.Status)
            .Include(s => s.Customer)
            .Include(s => s.Seller)
            .Include(s => s.SaleDetails)
                .ThenInclude(d => d.Sku)
                    .ThenInclude(sk => sk!.CompanyProduct)
                        .ThenInclude(cp => cp!.GlobalProduct)
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<SaleEntity> CreateAsync(SaleEntity entity)
    {
        var model = new Sale
        {
            CompanyId = entity.CompanyId,
            WarehouseId = entity.WarehouseId,
            SellerId = entity.SellerId,
            CustomerId = entity.CustomerId,
            StatusId = entity.StatusId,
            Notes = entity.Notes,
            SaleDate = entity.SaleDate,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.Sales.Add(model);
        await db.SaveChangesAsync();

        return await GetByIdAsync(model.Id) ?? Map(model);
    }

    public async Task UpdateStatusAsync(int id, int statusId)
    {
        var model = await db.Sales.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"Sale {id} not found");
        model.StatusId = statusId;
        await db.SaveChangesAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.Sales.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"Sale {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static SaleEntity Map(Sale m) =>
        new SaleEntity().Init(m.Id, m.CompanyId, m.WarehouseId, m.SellerId, m.CustomerId, m.StatusId, m.SaleDate ?? DateTime.Now, m.Notes, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now)
            .WithExpanded(
                m.Status is null ? null : new SaleStatusSummaryDto { Id = m.Status.Id, Code = m.Status.Code!, Name = m.Status.Name! },
                m.Customer is null ? null : new SalePersonSummaryDto { Id = m.Customer.Id, Name = m.Customer.Name!, Phone = m.Customer.Phone, Email = m.Customer.Email },
                m.Seller is null ? null : new SalePersonSummaryDto { Id = m.Seller.Id, Name = m.Seller.Name!, Phone = m.Seller.Phone },
                m.SaleDetails.Select(d => new SaleDetailExpandedDto
                {
                    Id = d.Id,
                    SkuId = d.SkuId,
                    BatchId = d.BatchId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    Subtotal = d.Quantity * d.UnitPrice,
                    Sku = d.Sku is null ? null : new SaleSkuExpandedDto
                    {
                        Id = d.Sku.Id,
                        InternalSku = d.Sku.InternalSku!,
                        RetailPrice = d.Sku.RetailPrice ?? 0,
                        ProductName = d.Sku.CompanyProduct?.GlobalProduct?.Name,
                        LocalNameAlias = d.Sku.CompanyProduct?.LocalNameAlias
                    }
                })
            );
}