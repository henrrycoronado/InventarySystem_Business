using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Contracts;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class InventoryService(IStockRepository stockRepository) : IInventoryService
{
    public async Task<decimal> GetAvailableStockAsync(int skuId, int warehouseId, int? batchId = null)
    {
        var stock = await stockRepository.GetBySkuAndWarehouseAsync(skuId, warehouseId, batchId);
        return stock?.AvailableQuantity ?? 0;
    }

    public async Task ReserveStockAsync(int skuId, int warehouseId, decimal quantity, int? batchId = null)
    {
        var stock = await stockRepository.GetBySkuAndWarehouseAsync(skuId, warehouseId, batchId)
            ?? throw new InvalidOperationException($"Stock not found for SKU {skuId} in warehouse {warehouseId}");
        stock.Reserve(quantity);
        await stockRepository.UpdateAsync(stock);
    }

    public async Task ConfirmReservationAsync(int skuId, int warehouseId, decimal quantity, int? batchId = null)
    {
        var stock = await stockRepository.GetBySkuAndWarehouseAsync(skuId, warehouseId, batchId)
            ?? throw new InvalidOperationException($"Stock not found for SKU {skuId} in warehouse {warehouseId}");
        stock.ConfirmReservation(quantity);
        await stockRepository.UpdateAsync(stock);
    }

    public async Task ReleaseReservationAsync(int skuId, int warehouseId, decimal quantity, int? batchId = null)
    {
        var stock = await stockRepository.GetBySkuAndWarehouseAsync(skuId, warehouseId, batchId)
            ?? throw new InvalidOperationException($"Stock not found for SKU {skuId} in warehouse {warehouseId}");
        stock.ReleaseReservation(quantity);
        await stockRepository.UpdateAsync(stock);
    }
}
