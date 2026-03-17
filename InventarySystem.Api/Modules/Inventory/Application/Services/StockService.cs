using InventarySystem.Api.Modules.Inventory.Application.DTOs;
using InventarySystem.Api.Modules.Inventory.Application.Interfaces;
using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class StockService(IStockRepository repository) : IStockService
{
    public async Task<IEnumerable<StockDto>> GetAllByWarehouseAsync(int warehouseId)
    {
        var items = await repository.GetAllByWarehouseAsync(warehouseId);
        return items.Select(Map);
    }

    public async Task<StockDto?> GetAvailableAsync(int skuId, int warehouseId, int? batchId)
    {
        var item = await repository.GetBySkuAndWarehouseAsync(skuId, warehouseId, batchId);
        return item is null ? null : Map(item);
    }

    public async Task<StockDto> CreateAsync(StockCreateDto dto)
    {
        var entity = StockEntity.Create(dto.WarehouseId, dto.SkuId, dto.BatchId, dto.Quantity);
        var created = await repository.CreateAsync(entity);
        return Map(created);
    }

    private static StockDto Map(StockEntity e) => new()
    {
        Id = e.Id, WarehouseId = e.WarehouseId, SkuId = e.SkuId, BatchId = e.BatchId,
        Quantity = e.Quantity, ReservedQuantity = e.ReservedQuantity,
        AvailableQuantity = e.AvailableQuantity, LastUpdated = e.LastUpdated,
        Sku = e.Sku
    };
}