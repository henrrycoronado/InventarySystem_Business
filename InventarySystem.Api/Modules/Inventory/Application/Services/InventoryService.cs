using InventarySystem.Api.Modules.Inventory.Domain.Entities;
using InventarySystem.Api.Modules.Inventory.Domain.Interfaces;
using InventarySystem.Api.src.Core.Contracts;

namespace InventarySystem.Api.Modules.Inventory.Application.Services;

public class InventoryService(
    IStockRepository stockRepository,
    IMovementRepository movementRepository,
    IMovementDetailRepository movementDetailRepository,
    IKardexRepository kardexRepository) : IInventoryService
{
    private const int ConfirmedStatusId = 2;

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

    public async Task RegisterOutgoingMovementAsync(int companyId, int warehouseId, int typeId, string? notes, List<(int skuId, int? batchId, decimal quantity, decimal? unitCost)> details)
    {
        var movement = MovementEntity.Create(companyId, warehouseId, null, ConfirmedStatusId, typeId, notes);
        var createdMovement = await movementRepository.CreateAsync(movement);

        foreach (var (skuId, batchId, quantity, unitCost) in details)
        {
            var movementDetail = MovementDetailEntity.Create(createdMovement.Id, skuId, batchId, quantity, unitCost);
            var createdDetail = await movementDetailRepository.CreateAsync(movementDetail);

            var stock = await stockRepository.GetBySkuAndWarehouseAsync(skuId, warehouseId, batchId);
            var balanceAfter = stock?.AvailableQuantity ?? 0;

            var kardex = KardexEntity.Create(companyId, warehouseId, skuId, batchId, createdDetail.Id, typeId, quantity, balanceAfter);
            await kardexRepository.CreateAsync(kardex);
        }
    }
}