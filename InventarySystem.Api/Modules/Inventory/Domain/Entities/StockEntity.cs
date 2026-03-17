using InventarySystem.Api.Modules.Inventory.Application.DTOs;

namespace InventarySystem.Api.Modules.Inventory.Domain.Entities;

public class StockEntity
{
    public int Id { get; private set; }
    public int WarehouseId { get; private set; }
    public int SkuId { get; private set; }
    public int? BatchId { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal ReservedQuantity { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime LastUpdated { get; private set; }
    public SkuExpandedDto? Sku { get; private set; }

    public decimal AvailableQuantity => Quantity - ReservedQuantity;

    internal StockEntity() { }

    public static StockEntity Create(int warehouseId, int skuId, int? batchId, decimal quantity) =>
        new() { WarehouseId = warehouseId, SkuId = skuId, BatchId = batchId, Quantity = quantity, ReservedQuantity = 0, IsActive = true, LastUpdated = DateTime.Now };

    public void Reserve(decimal qty)
    {
        if (qty > AvailableQuantity)
            throw new InvalidOperationException($"Insufficient stock. Available: {AvailableQuantity}, Requested: {qty}");
        ReservedQuantity += qty;
        LastUpdated = DateTime.Now;
    }

    public void ConfirmReservation(decimal qty)
    {
        Quantity -= qty;
        ReservedQuantity -= qty;
        LastUpdated = DateTime.Now;
    }

    public void ReleaseReservation(decimal qty)
    {
        ReservedQuantity -= qty;
        LastUpdated = DateTime.Now;
    }

    public void Deactivate() => IsActive = false;

    internal StockEntity Init(int id, int warehouseId, int skuId, int? batchId, decimal quantity, decimal reservedQuantity, bool isActive, DateTime lastUpdated)
    {
        Id = id; WarehouseId = warehouseId; SkuId = skuId; BatchId = batchId;
        Quantity = quantity; ReservedQuantity = reservedQuantity;
        IsActive = isActive; LastUpdated = lastUpdated;
        return this;
    }

    internal StockEntity WithExpanded(SkuExpandedDto? sku)
    {
        Sku = sku;
        return this;
    }
}