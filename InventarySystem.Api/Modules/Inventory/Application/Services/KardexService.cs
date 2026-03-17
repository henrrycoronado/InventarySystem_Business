using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class KardexService(IKardexRepository repository, AppDbContext db) : IKardexService
{
    public async Task<KardexResponseDto?> GetBySkuAndWarehouseAsync(int skuId, int warehouseId)
    {
        var sku = await db.CompanySkus
            .Include(s => s.CompanyProduct)
                .ThenInclude(cp => cp!.GlobalProduct)
            .FirstOrDefaultAsync(s => s.Id == skuId && s.IsActive == true);

        if (sku is null) return null;

        var movementTypes = await db.MovementTypes.ToListAsync();
        var entries = await repository.GetBySkuAndWarehouseAsync(skuId, warehouseId);

        return new KardexResponseDto
        {
            SkuId = skuId,
            InternalSku = sku.InternalSku!,
            ProductName = sku.CompanyProduct?.GlobalProduct?.Name ?? sku.InternalSku!,
            Entries = entries.Select(e => new KardexEntryDto
            {
                Id = e.Id,
                Date = e.CreatedAt,
                TypeId = e.TypeId,
                TypeName = movementTypes.FirstOrDefault(t => t.Id == e.TypeId)?.Name ?? "",
                Quantity = e.Quantity,
                BalanceAfter = e.BalanceAfter
            })
        };
    }
}