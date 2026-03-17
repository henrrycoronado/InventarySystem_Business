using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Inventory.Infrastructure.Repositories;

public class CompanyProductRepository(AppDbContext db) : ICompanyProductRepository
{
    public async Task<IEnumerable<CompanyProductEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.CompanyProducts
            .Include(p => p.GlobalProduct)
                .ThenInclude(gp => gp!.Category)
            .Include(p => p.CompanySkus.Where(s => s.IsActive == true))
            .Where(p => p.CompanyId == companyId && p.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<CompanyProductEntity?> GetByIdAsync(int id, int companyId)
    {
        var model = await db.CompanyProducts
            .Include(p => p.GlobalProduct)
                .ThenInclude(gp => gp!.Category)
            .Include(p => p.CompanySkus.Where(s => s.IsActive == true))
            .FirstOrDefaultAsync(p => p.Id == id && p.CompanyId == companyId && p.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<CompanyProductEntity> CreateAsync(CompanyProductEntity entity)
    {
        var model = new CompanyProduct
        {
            CompanyId = entity.CompanyId,
            GlobalProductId = entity.GlobalProductId,
            LocalNameAlias = entity.LocalNameAlias,
            WholesalePrice = entity.WholesalePrice,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.CompanyProducts.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.CompanyProducts.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"CompanyProduct {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static CompanyProductEntity Map(CompanyProduct m) =>
        new CompanyProductEntity().Init(m.Id, m.CompanyId, m.GlobalProductId ?? 0, m.LocalNameAlias, m.WholesalePrice ?? 0, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now)
            .WithExpanded(
                m.GlobalProduct is null ? null : new()
                {
                    Id = m.GlobalProduct.Id,
                    Name = m.GlobalProduct.Name!,
                    Brand = m.GlobalProduct.Brand,
                    UpcBarcode = m.GlobalProduct.UpcBarcode,
                    CategoryId = m.GlobalProduct.CategoryId,
                    Category = m.GlobalProduct.Category is null ? null : new()
                    {
                        Id = m.GlobalProduct.Category.Id,
                        Name = m.GlobalProduct.Category.Name!
                    }
                },
                m.CompanySkus.Select(s => new CompanySkuSummary(s.Id, s.InternalSku!, s.RetailPrice ?? 0))
            );
}