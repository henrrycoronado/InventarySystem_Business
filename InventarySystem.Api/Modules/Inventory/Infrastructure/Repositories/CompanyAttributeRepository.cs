using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Inventory.Infrastructure.Repositories;

public class CompanyAttributeRepository(AppDbContext db) : ICompanyAttributeRepository
{
    public async Task<IEnumerable<CompanyAttributeEntity>> GetAllByCompanyAsync(int companyId)
    {
        var models = await db.CompanyAttributes
            .Where(a => a.CompanyId == companyId && a.IsActive == true)
            .ToListAsync();

        var attributeIds = models.Select(a => a.Id).ToList();
        var skuCounts = await db.SkuAttributeValues
            .Where(v => attributeIds.Contains(v.AttributeId) && v.IsActive == true)
            .GroupBy(v => v.AttributeId)
            .Select(g => new { AttributeId = g.Key, Count = g.Count() })
            .ToListAsync();

        return models.Select(m =>
        {
            var count = skuCounts.FirstOrDefault(s => s.AttributeId == m.Id)?.Count ?? 0;
            return new CompanyAttributeEntity().Init(m.Id, m.CompanyId, m.Name!, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now)
                .WithSkuCount(count);
        });
    }

    public async Task<CompanyAttributeEntity?> GetByIdAsync(int id, int companyId)
    {
        var model = await db.CompanyAttributes
            .FirstOrDefaultAsync(a => a.Id == id && a.CompanyId == companyId && a.IsActive == true);
        return model is null ? null : Map(model);
    }

    public async Task<CompanyAttributeEntity> CreateAsync(CompanyAttributeEntity entity)
    {
        var model = new CompanyAttribute
        {
            CompanyId = entity.CompanyId,
            Name = entity.Name,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.CompanyAttributes.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.CompanyAttributes.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"CompanyAttribute {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static CompanyAttributeEntity Map(CompanyAttribute m) =>
        new CompanyAttributeEntity().Init(m.Id, m.CompanyId, m.Name!, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now);
}
