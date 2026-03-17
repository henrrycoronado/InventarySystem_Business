using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using InventarySystem.Api.src.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Inventory.Infrastructure.Repositories;

public class SkuAttributeValueRepository(AppDbContext db) : ISkuAttributeValueRepository
{
    public async Task<IEnumerable<SkuAttributeValueEntity>> GetAllBySkuAsync(int skuId)
    {
        var models = await db.SkuAttributeValues
            .Include(v => v.Attribute)
            .Where(v => v.SkuId == skuId && v.IsActive == true)
            .ToListAsync();
        return models.Select(Map);
    }

    public async Task<SkuAttributeValueEntity> CreateAsync(SkuAttributeValueEntity entity)
    {
        var model = new SkuAttributeValue
        {
            SkuId = entity.SkuId,
            AttributeId = entity.AttributeId,
            Value = entity.Value,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt
        };
        db.SkuAttributeValues.Add(model);
        await db.SaveChangesAsync();
        return Map(model);
    }

    public async Task DeactivateAsync(int id)
    {
        var model = await db.SkuAttributeValues.FindAsync(id);
        if (model is null) throw new KeyNotFoundException($"SkuAttributeValue {id} not found");
        model.IsActive = false;
        await db.SaveChangesAsync();
    }

    private static SkuAttributeValueEntity Map(SkuAttributeValue m) =>
        new SkuAttributeValueEntity().Init(m.Id, m.SkuId, m.AttributeId, m.Value!, m.IsActive ?? true, m.CreatedAt ?? DateTime.Now)
            .WithAttribute(m.Attribute is null ? null : new AttributeSummary(m.Attribute.Id, m.Attribute.Name!));
}
