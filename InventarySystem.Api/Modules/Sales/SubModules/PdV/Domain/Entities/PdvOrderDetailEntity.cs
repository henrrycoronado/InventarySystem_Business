namespace InventarySystem.Api.Modules.Sales.SubModules.PdV.Domain.Entities;

public class PdvOrderDetailEntity
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int MenuItemId { get; private set; }
    public int? StationId { get; private set; }
    public int StatusId { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Notes { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    internal PdvOrderDetailEntity() { }

    public static PdvOrderDetailEntity Create(int orderId, int menuItemId, int? stationId, int statusId, decimal quantity, decimal unitPrice, string? notes) =>
        new() { OrderId = orderId, MenuItemId = menuItemId, StationId = stationId, StatusId = statusId, Quantity = quantity, UnitPrice = unitPrice, Notes = notes, IsActive = true, CreatedAt = DateTime.Now };

    public void UpdateStatus(int statusId) => StatusId = statusId;
    public void Deactivate() => IsActive = false;

    internal PdvOrderDetailEntity Init(int id, int orderId, int menuItemId, int? stationId, int statusId, decimal quantity, decimal unitPrice, string? notes, bool isActive, DateTime createdAt)
    {
        Id = id; OrderId = orderId; MenuItemId = menuItemId; StationId = stationId;
        StatusId = statusId; Quantity = quantity; UnitPrice = unitPrice;
        Notes = notes; IsActive = isActive; CreatedAt = createdAt;
        return this;
    }
}