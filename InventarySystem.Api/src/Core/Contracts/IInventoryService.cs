namespace InventarySystem.Api.src.Core.Contracts;

public interface IInventoryService
{
    Task<decimal> GetAvailableStockAsync(int skuId, int warehouseId, int? batchId = null);
    Task ReserveStockAsync(int skuId, int warehouseId, decimal quantity, int? batchId = null);
    Task ConfirmReservationAsync(int skuId, int warehouseId, decimal quantity, int? batchId = null);
    Task ReleaseReservationAsync(int skuId, int warehouseId, decimal quantity, int? batchId = null);
    Task RegisterOutgoingMovementAsync(int companyId, int warehouseId, int typeId, string? notes, List<(int skuId, int? batchId, decimal quantity, decimal? unitCost)> details);
}
